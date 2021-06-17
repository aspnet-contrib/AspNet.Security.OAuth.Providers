/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.StackExchange
{
    public class StackExchangeAuthenticationHandler : OAuthHandler<StackExchangeAuthenticationOptions>
    {
        public StackExchangeAuthenticationHandler(
            [NotNull] IOptionsMonitor<StackExchangeAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            if (string.IsNullOrEmpty(Options.Site))
            {
                throw new InvalidOperationException(
                    $"No site was specified for the {nameof(StackExchangeAuthenticationOptions.Site)} property of {nameof(StackExchangeAuthenticationOptions)}.");
            }

            var queryArguments = new Dictionary<string, string?>
            {
                ["access_token"] = tokens.AccessToken,
                ["site"] = Options.Site,
            };

            if (!string.IsNullOrEmpty(Options.RequestKey))
            {
                queryArguments["key"] = Options.RequestKey;
            }

            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, queryArguments);

            using var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
            context.RunClaimActions(payload.RootElement.GetProperty("items").EnumerateArray().First());

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            var parameters = new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["redirect_uri"] = context.RedirectUri,
                ["client_secret"] = Options.ClientSecret,
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code"
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(parameters!)
            };

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            // Note: StackExchange's token endpoint doesn't return JSON but uses application/x-www-form-urlencoded.
            // Since OAuthTokenResponse expects a JSON payload, a response is manually created using the returned values.
            var content = QueryHelpers.ParseQuery(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var node = new JsonObject();

            foreach ((string key, StringValues value) in content)
            {
                node[key] = value.ToString();
            }

            var copy = JsonDocument.Parse(node.ToJsonString());
            return OAuthTokenResponse.Success(copy);
        }
    }
}
