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
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.LinkedIn {
    public class LinkedInAuthenticationHandler : OAuthAuthenticationHandler<LinkedInAuthenticationOptions> {
        public LinkedInAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, LinkedInAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, LinkedInAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Email, LinkedInAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:linkedin:profile", LinkedInAuthenticationHelper.GetPublicProfileUrl(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:linkedin:profilepicture", LinkedInAuthenticationHelper.GetProfilePictureUrl(payload), Options.ClaimsIssuer);

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