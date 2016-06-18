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
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Authentication.Yammer {
    public class YammerAuthenticationHandler : OAuthHandler<YammerAuthenticationOptions> {
        public YammerAuthenticationHandler(HttpClient httpClient)
            : base(httpClient) {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri) {
            /*
             https://developer.yammer.com/docs/oauth-2
             Override this method because Yamer API returns unusual response for TokenEndpoint request
             */
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode) {
                var accessTokenObject = JObject.Parse(await response.Content.ReadAsStringAsync())["access_token"].Value<JObject>();

                JObject payload = JObject.FromObject(new {
                    access_token = accessTokenObject["token"],
                    token_type = string.Empty,
                    refresh_token = string.Empty,
                    expires_in = string.Empty
                });

                return OAuthTokenResponse.Success(payload);
            } else {
                var error = "OAuth token endpoint failure: " + await Display(response);
                return OAuthTokenResponse.Failed(new Exception(error));
            }
        }

        private async Task<string> Display(HttpResponseMessage response) {
            var output = new StringBuilder();
            output.Append("Status: " + response.StatusCode + ";");
            output.Append("Headers: " + response.Headers + ";");
            output.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
            return output.ToString();
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens) {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);

            if (!response.IsSuccessStatusCode) {
                Logger.LogError("An error occurred when retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                    /* Status: */ response.StatusCode,
                    /* Headers: */ response.Headers.ToString(),
                    /* Body: */ await response.Content.ReadAsStringAsync());
                throw new HttpRequestException("An error occurred when retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, YammerAuthenticationHelper.GetId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, YammerAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, YammerAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, YammerAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, YammerAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:yammer:link", YammerAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:yammer:job_title", YammerAuthenticationHelper.GetJobTitle(payload), Options.ClaimsIssuer);

            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}