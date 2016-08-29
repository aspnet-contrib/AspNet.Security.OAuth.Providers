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
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.VisualStudio {
    public class VisualStudioAuthenticationHandler : OAuthHandler<VisualStudioAuthenticationOptions> {
        public VisualStudioAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
           ClaimsIdentity identity,
           AuthenticationProperties properties, OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode) {
                Logger.LogError("An error occurred when retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred when retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, VisualStudioAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, VisualStudioAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, VisualStudioAuthenticationHelper.GetLogin(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, VisualStudioAuthenticationHelper.GetName(payload), Options.ClaimsIssuer);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri) {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "redirect_uri", redirectUri },
                { "client_assertion", Options.ClientSecret },
                { "assertion", code },
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);

            if (!response.IsSuccessStatusCode) {
                Logger.LogError("An error occurred when retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred when retrieving an access token."));
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri) {
            var scope = FormatScope();
            var state = Options.StateDataFormat.Protect(properties);

            var queryBuilder = new QueryBuilder
            {
                { "client_id", Options.ClientId },
                { "response_type", "Assertion" },
                { "scope", scope },
                { "redirect_uri", redirectUri },
                { "state", state },
            };
            return Options.AuthorizationEndpoint + queryBuilder;
        }
    }
}