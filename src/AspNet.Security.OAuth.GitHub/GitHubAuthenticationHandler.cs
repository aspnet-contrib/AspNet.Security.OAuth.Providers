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
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Extensions.Internal;
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
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, GitHubAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, GitHubAuthenticationHelper.GetLogin(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, GitHubAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:name", GitHubAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:url", GitHubAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer);

            // When the email address is not public, retrieve it from the emails endpoint if the user:email scope is specified.
            if (!identity.HasClaim(claim => claim.Type == ClaimTypes.Email) && Options.Scope.Contains("user:email")) {
                identity.AddOptionalClaim(ClaimTypes.Email, await GetEmailAsync(tokens), Options.ClaimsIssuer);
            }

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null) {
                return null;
            }
                    
            return new AuthenticationTicket(context.Principal, context.Properties, Options.AuthenticationScheme);
        }

        protected virtual async Task<string> GetEmailAsync([NotNull] OAuthTokenResponse tokens) {
            // See https://developer.github.com/v3/users/emails/ for more information about the /user/emails endpoint.
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint + "/emails");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            // Failed requests shouldn't cause an error: in this case, return null to indicate that the email address cannot be retrieved.
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode) {
                return null;
            }

            var payload = JArray.Parse(await response.Content.ReadAsStringAsync());

            return GitHubAuthenticationHelper.GetEmail(payload);
        }
    }
}
