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

        protected override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query;
            if (query.TryGetValue("auth_code", out var authCode))
            {
                // The base `HandleRemoteAuthenticateAsync` requires that `Request.Query` must contain the key called `code`,
                // which is actually the same as `auth_code` by Alipay's design, but `Request.Query` does not have `Add` operation.
                // So here is a trick to get the private `Store` dictionary of `QueryCollection`.
                var queryStore = query.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
                queryStore["code"] = authCode;
                Request.QueryString = QueryString.Create(queryStore);
            }

            return base.HandleRemoteAuthenticateAsync();
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            // See https://opendocs.alipay.com/apis/api_9/alipay.system.oauth.token for details.
            var sortedParams = new SortedDictionary<string, string?>()
            {
                ["app_id"] = Options.ClientId,
                ["charset"] = "utf-8",
                ["code"] = context.Code,
                ["format"] = "JSON",
                ["grant_type"] = "authorization_code",
                ["method"] = "alipay.system.oauth.token",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                ["version"] = "1.0",
            };
            sortedParams.Add("sign", GetRSA2Signature(sortedParams));

            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, sortedParams!);

            using var response = await Backchannel.GetAsync(address, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
            using var document = JsonDocument.Parse(stream);

            var mainElement = document.RootElement.GetProperty("alipay_system_oauth_token_response");
            if (!ValidateReturnCode(mainElement, out string code))
            {
                return OAuthTokenResponse.Failed(new Exception($"An error (Code:{code}) occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(mainElement.GetRawText());
            return OAuthTokenResponse.Success(payload);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            // See https://opendocs.alipay.com/apis/api_2/alipay.user.info.share for details.
            var sortedParams = new SortedDictionary<string, string?>()
            {
                ["app_id"] = Options.ClientId,
                ["auth_token"] = tokens.AccessToken,
                ["charset"] = "utf-8",
                ["format"] = "JSON",
                ["method"] = "alipay.user.info.share",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                ["version"] = "1.0",
            };
            sortedParams.Add("sign", GetRSA2Signature(sortedParams));

            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, sortedParams!);

            using var response = await Backchannel.GetAsync(address, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
            using var document = JsonDocument.Parse(stream);
            var rootElement = document.RootElement;

            if (!rootElement.TryGetProperty("alipay_user_info_share_response", out JsonElement mainElement))
            {
                string errorCode = rootElement.GetProperty("error_response").GetProperty("code").GetString() !;
                throw new Exception($"An error (Code:{errorCode}) occurred while retrieving user information.");
            }

            if (!ValidateReturnCode(mainElement, out string code))
            {
                throw new Exception($"An error (Code:{code}) occurred while retrieving user information.");
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, mainElement.GetString("user_id") !, ClaimValueTypes.String, Options.ClaimsIssuer));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, mainElement);

            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override string FormatScope([NotNull] IEnumerable<string> scopes) => string.Join(',', Options.Scope);

        /// <summary>
        /// Check the code sent back by server for potential server errors.
        /// </summary>
        /// <param name="element">Main part of json document from response</param>
        /// <param name="code">Returned code from server</param>
        /// <remarks>See https://opendocs.alipay.com/open/common/105806 for details.</remarks>
        /// <returns>True if succeed, otherwise false.</returns>
        private static bool ValidateReturnCode(JsonElement element, out string code)
        {
            if (!element.TryGetProperty("code", out JsonElement codeElement))
            {
                code = string.Empty;
                return true;
            }

            code = codeElement.GetString() !;
            return code == "10000";
        }

        /// <summary>
        /// Gets the RSA2(SHA256 with RSA) signature.
        /// </summary>
        /// <param name="sortedPairs">Sorted key-value pairs</param>
        private string GetRSA2Signature([NotNull] SortedDictionary<string, string?> sortedPairs)
        {
            var builder = new StringBuilder(128);

            foreach (var pair in sortedPairs)
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

            string plainText = builder.ToString();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText, 0, plainText.Length - 1); // Skip the last '&'
            byte[] privateKeyBytes = Convert.FromBase64String(Options.ClientSecret);

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            byte[] encryptedBytes = rsa.SignData(plainBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(encryptedBytes);
        }

         /// <inheritdoc />
        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            var scopeParameter = properties.GetParameter<ICollection<string>>(OAuthChallengeProperties.ScopeKey);
            var scope = scopeParameter != null ? FormatScope(scopeParameter) : FormatScope();

            var parameters = new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId, // Used instead of "client_id"
                ["scope"] = scope,
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
            };

            if (Options.UsePkce)
            {
                var bytes = new byte[32];
                RandomNumberGenerator.Fill(bytes);
                var codeVerifier = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(bytes);

                // Store this for use during the code redemption.
                properties.Items.Add(OAuthConstants.CodeVerifierKey, codeVerifier);

                var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
                var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);

                parameters[OAuthConstants.CodeChallengeKey] = codeChallenge;
                parameters[OAuthConstants.CodeChallengeMethodKey] = OAuthConstants.CodeChallengeMethodS256;
            }

            parameters["state"] = Options.StateDataFormat.Protect(properties);

            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters!);
        }
    }
}
