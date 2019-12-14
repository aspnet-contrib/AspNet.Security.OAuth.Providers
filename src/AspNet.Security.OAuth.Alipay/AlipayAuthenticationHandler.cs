/* 
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0) 
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers 
 * for more information concerning the license and the contributors participating to this project. 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Alipay
{
    /// <summary>
    /// Defines a handler for authentication using Alipay.
    /// </summary>
    public class AlipayAuthenticationHandler : OAuthHandler<AlipayAuthenticationOptions>
    {
        public AlipayAuthenticationHandler(
            [NotNull] IOptionsMonitor<AlipayAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);

            var address = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId,
                ["scope"] = string.Join(",", Options.Scope),
                ["redirect_uri"] = redirectUri,
                ["state"] = stateValue
            });

            return address;
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);

            if (query.TryGetValue("auth_code", out var authCode))
            {
                query["code"] = authCode;
                Request.QueryString = QueryString.Create(query);
            }

            return await base.HandleRemoteAuthenticateAsync();
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            var sortedParams = new SortedDictionary<string, string>()
            {
                ["app_id"] = Options.ClientId,
                ["method"] = "alipay.system.oauth.token",
                ["format"] = "JSON",
                ["charset"] = "UTF-8",
                ["sign_type"] = "RSA2",
                ["version"] = "1.0",
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code",
            };

            if (Options.ValidateSignature)
            {
                sortedParams.Add("timestamp", Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                sortedParams.Add("sign", getRSA2Signature(sortedParams, Options.ClientSecret));
            }

            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, sortedParams);

            using var request = new HttpRequestMessage(HttpMethod.Get, address);
            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            using var payload = await getPayloadAsync(response, "alipay_system_oauth_token_response");

            return OAuthTokenResponse.Success(payload);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var sortedParams = new SortedDictionary<string, string>()
            {
                ["app_id"] = Options.ClientId,
                ["method"] = "alipay.user.info.share",
                ["format"] = "JSON",
                ["charset"] = "UTF-8",
                ["sign_type"] = "RSA2",
                ["version"] = "1.0",
                ["auth_token"] = tokens.AccessToken,
            };

            if (Options.ValidateSignature)
            {
                sortedParams.Add("timestamp", Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                sortedParams.Add("sign", getRSA2Signature(sortedParams, Options.ClientSecret));
            }

            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, sortedParams);

            using var response = await Backchannel.GetAsync(address);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            using var payload = await getPayloadAsync(response, "alipay_user_info_share_response");
            string statusCode = payload.RootElement.GetString("code");

            if (statusCode != "10000")/*  */
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                            "returned a {Status} response with the following message: {Message}.",
                            /* Status: */ statusCode,
                            /* Message: */ payload.RootElement.GetString("msg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, payload.RootElement.GetString("user_id"), ClaimValueTypes.String, Options.ClaimsIssuer));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);

            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        /// <summary>
        /// get payload as an asynchronous operation.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>Task&lt;JsonDocument&gt;.</returns>
        private async Task<JsonDocument> getPayloadAsync(HttpResponseMessage message, string nodeName)
        {
            var messageContent = await message.Content.ReadAsStringAsync();

            //Logger.LogInformation(messageContent);

            var rootJson = JsonDocument.Parse(messageContent);
            var payload = JsonDocument.Parse(rootJson.RootElement.GetString(nodeName));

            return payload;
        }

        /// <summary>
        /// Gets the RSA2 signature.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>System.String.</returns>
        private string getRSA2Signature(SortedDictionary<string, string> source, [NotNull] string privateKey)
        {
            var textParams = source
                .Where(p => !string.Equals(p.Key, "sign", StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(p.Value))
                .Select(p => p.Key + "=" + p.Value);

            var privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);

            var dataBytes = Encoding.UTF8.GetBytes(string.Join("&", textParams));
            var signatureBytes = privateKeyRsaProvider.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// Creates the RSA provider from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>RSA.</returns>
        /// <exception cref="Exception">
        /// Unexpected value read binr.ReadUInt16()
        /// or
        /// Unexpected version
        /// or
        /// Unexpected value read binr.ReadByte()
        /// </exception>
        private RSA CreateRsaProviderFromPrivateKey(string privateKey)
        {
            var privateKeyBits = Convert.FromBase64String(privateKey);

            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(rsaParameters);
            return rsa;
        }

        /// <summary>
        /// Gets the size of the integer.
        /// </summary>
        /// <param name="binr">The binr.</param>
        /// <returns>System.Int32.</returns>
        private int GetIntegerSize(BinaryReader binr)
        {
            byte bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            int count;
            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
    }
}
