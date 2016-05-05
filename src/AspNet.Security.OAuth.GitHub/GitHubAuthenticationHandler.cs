/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

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

namespace AspNet.Security.OAuth.GitHub {
    public class GitHubAuthenticationHandler : OAuthHandler<GitHubAuthenticationOptions> {
        public GitHubAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
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
            
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, GitHubAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, GitHubAuthenticationHelper.GetLogin(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, GitHubAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:name", GitHubAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:url", GitHubAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer);

            // When the email address is not public, retrieve it from
            // the emails endpoint if the user:email scope is specified.
            if (!string.IsNullOrEmpty(Options.UserEmailsEndpoint) &&
                !identity.HasClaim(claim => claim.Type == ClaimTypes.Email) && Options.Scope.Contains("user:email")) {
                identity.AddOptionalClaim(ClaimTypes.Email, await GetEmailAsync(tokens), Options.ClaimsIssuer);
            }

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected virtual async Task<string> GetEmailAsync([NotNull] OAuthTokenResponse tokens) {
            // See https://developer.github.com/v3/users/emails/ for more information about the /user/emails endpoint.
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserEmailsEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            // Failed requests shouldn't cause an error: in this case, return null to indicate that the email address cannot be retrieved.
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode) {
                Logger.LogWarning("An error occurred when retrieving the email address associated with the logged in user: " +
                                  "the remote server returned a {Status} response with the following payload: {Headers} {Body}.",
                                  /* Status: */ response.StatusCode,
                                  /* Headers: */ response.Headers.ToString(),
                                  /* Body: */ await response.Content.ReadAsStringAsync());

                return null;
            }

            var payload = JArray.Parse(await response.Content.ReadAsStringAsync());

            return GitHubAuthenticationHelper.GetEmail(payload);
        }
    }
}
