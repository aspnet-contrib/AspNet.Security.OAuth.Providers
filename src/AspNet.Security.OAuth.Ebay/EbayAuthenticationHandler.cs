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
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace AspNet.Security.OAuth.Ebay
{
    public class EbayAuthenticationHandler : OAuthHandler<EbayAuthenticationOptions>
    {
        public EbayAuthenticationHandler(
            IOptionsMonitor<EbayAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, EbayAuthenticationDefaults.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await this.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, this.Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(this.Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(this.Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, this.Context, this.Scheme, this.Options, this.Backchannel, tokens, payload.RootElement);
            context.RunClaimActions(payload.RootElement);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, this.Scheme.Name);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var parameters = new Dictionary<string, string>
            {
                ["client_id"] = this.Options.ClientId,
                ["redirect_uri"] = this.Options.RedirectUrl,
                ["response_type"] = "code",
                ["scope"] = this.FormatScope(this.Options.Scope.Distinct().Select(s => NormalizeScope(s)))
            };
            parameters["state"] = this.Options.StateDataFormat.Protect(properties);

            return QueryHelpers.AddQueryString(this.Options.AuthorizationEndpoint, parameters!);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, this.Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = CreateAuthorizationHeader();

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = context.Code,
                ["redirect_uri"] = this.Options.RedirectUrl
            };

            request.Content = new FormUrlEncodedContent(parameters!);

            using var response = await this.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, this.Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(this.Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(this.Context.RequestAborted));

            return OAuthTokenResponse.Success(payload);
        }

        private AuthenticationHeaderValue CreateAuthorizationHeader()
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                string.Concat(
                    EscapeDataString(this.Options.ClientId),
                    ":",
                    EscapeDataString(this.Options.ClientSecret))));

            return new AuthenticationHeaderValue("Basic", credentials);
        }

        private static string EscapeDataString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return Uri.EscapeDataString(value).Replace("%20", "+", StringComparison.Ordinal);
        }

        private static string NormalizeScope(string scope)
        {
            if (!scope.StartsWith(EbayAuthenticationDefaults.ScopePrefix, StringComparison.OrdinalIgnoreCase))
            {
                scope = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", EbayAuthenticationDefaults.ScopePrefix, scope);
            }

            return scope;
        }
    }
}
