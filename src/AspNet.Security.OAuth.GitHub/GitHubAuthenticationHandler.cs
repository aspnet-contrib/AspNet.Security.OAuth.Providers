using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.GitHub {
    public class GitHubAuthenticationHandler : OAuthAuthenticationHandler<GitHubAuthenticationOptions> {
        public GitHubAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, GitHubAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, GitHubAuthenticationHelper.GetLogin(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:name", GitHubAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:github:url", GitHubAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer);

            var notification = new OAuthAuthenticatedContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Notifications.Authenticated(notification);

            if (notification.Principal?.Identity == null) {
                return null;
            }
                    
            return new AuthenticationTicket(
                notification.Principal, notification.Properties,
                notification.Options.AuthenticationScheme);
        }
    }
}
