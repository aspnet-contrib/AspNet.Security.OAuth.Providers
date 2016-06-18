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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Yammer {
    public class YammerAuthenticationHandler : OAuthHandler<YammerAuthenticationOptions> {
        public YammerAuthenticationHandler(HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode) {
                Logger.LogError("An error occurred when retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred when retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, YammerAuthenticationHelper.GetId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, YammerAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, YammerAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, YammerAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, YammerAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:yammer:link", YammerAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:yammer:job_title", YammerAuthenticationHelper.GetJobTitle(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri) {
            var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                ["client_id"] = Options.ClientId,
                ["redirect_uri"] = redirectUri,
                ["client_secret"] = Options.ClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code"
            });

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode) {
                Logger.LogError("An error occurred when retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred when retrieving an access token."));
            }

            // Note: Yammer doesn't return a standard OAuth2 response. To make this middleware compatible
            // with the OAuth2 generic middleware, a compliant JSON payload is generated manually.
            // See https://developer.yammer.com/docs/oauth-2 for more information about this process.
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync())["access_token"].Value<JObject>();
            payload["access_token"] = payload["token"];
            payload["token_type"] = string.Empty;
            payload["refresh_token"] = string.Empty;
            payload["expires_in"] = string.Empty;

            return OAuthTokenResponse.Success(payload);
        }
    }
}