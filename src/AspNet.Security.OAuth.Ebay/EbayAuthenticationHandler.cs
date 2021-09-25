/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions(payload.RootElement);

            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            var parameters = new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["redirect_uri"] = Options.RuName!,
                ["response_type"] = "code",
                ["scope"] = FormatScope(Options.Scope)
            };
            parameters["state"] = Options.StateDataFormat.Protect(properties);

            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters!);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = CreateAuthorizationHeader();

            var parameters = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = context.Code,
                ["redirect_uri"] = Options.RuName!
            };

            request.Content = new FormUrlEncodedContent(parameters!);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            return OAuthTokenResponse.Success(payload);
        }

        private AuthenticationHeaderValue CreateAuthorizationHeader()
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                string.Concat(
                    EscapeDataString(Options.ClientId),
                    ":",
                    EscapeDataString(Options.ClientSecret))));

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
    }
}
