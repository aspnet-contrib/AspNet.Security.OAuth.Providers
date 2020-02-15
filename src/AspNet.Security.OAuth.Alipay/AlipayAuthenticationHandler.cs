/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
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

        protected override string BuildChallengeUrl(
            [NotNull] AuthenticationProperties properties,
            [NotNull] string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);

            string address = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId,
                ["redirect_uri"] = redirectUri,
                ["scope"] = FormatScope(),
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
                ["charset"] = "UTF-8",
                ["code"] = context.Code,
                ["format"] = "JSON",
                ["grant_type"] = "authorization_code",
                ["method"] = "alipay.system.oauth.token",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                ["version"] = "1.0",
            };
            sortedParams.Add("sign", GetRSA2Signature(sortedParams));

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

            var payload = await ReadJsonDocumentAsync(response, "alipay_system_oauth_token_response");

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
                ["auth_token"] = tokens.AccessToken,
                ["charset"] = "UTF-8",
                ["format"] = "JSON",
                ["method"] = "alipay.user.info.share",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                ["version"] = "1.0",
            };
            sortedParams.Add("sign", GetRSA2Signature(sortedParams));

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

            var payload = await ReadJsonDocumentAsync(response, "alipay_user_info_share_response");

            string statusCode = payload.RootElement.GetString("code");

            // if code==10000 then success else failed. See: https://docs.open.alipay.com/common/105806
            if (statusCode != "10000")
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

        protected override string FormatScope() => string.Join(",", Options.Scope);

        private static async Task<JsonDocument> ReadJsonDocumentAsync(
            [NotNull] HttpResponseMessage message,
            [NotNull] string propertyName)
        {
            using var document = JsonDocument.Parse(await message.Content.ReadAsStringAsync());
            string json = document.RootElement.GetProperty(propertyName).ToString();

            return JsonDocument.Parse(json);
        }

        /// <summary>
        /// Gets the RSA2 signature.
        /// </summary>
        /// <param name="source">The source.</param>
        private string GetRSA2Signature([NotNull] SortedDictionary<string, string> source)
        {
            var builder = new StringBuilder();

            foreach (var pair in source)
            {
                if (string.IsNullOrEmpty(pair.Value))
                {
                    continue;
                }

                builder.Append(pair.Key)
                       .Append('=')
                       .Append(pair.Value)
                       .Append('&');
            }

            builder.Remove(builder.Length - 1, 1);

            byte[] dataBytes = Encoding.UTF8.GetBytes(builder.ToString());

            using var privateKeyRsaProvider = CreateAlgorithm();
            byte[] signatureBytes = privateKeyRsaProvider.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// Creates the algorithm.
        /// </summary>
        private RSA CreateAlgorithm()
        {
            byte[] privateKeyBytes = Convert.FromBase64String(Options.ClientSecret);
            var algorithm = RSA.Create();

            try
            {
                algorithm.ImportRSAPrivateKey(privateKeyBytes, out int _);
                return algorithm;
            }
            catch (Exception)
            {
                algorithm.Dispose();
                throw;
            }
        }
    }
}
