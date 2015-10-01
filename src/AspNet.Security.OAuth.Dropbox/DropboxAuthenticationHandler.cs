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

namespace AspNet.Security.OAuth.Dropbox {
    public class DropboxAuthenticationHandler : OAuthHandler<DropboxAuthenticationOptions> {
        public DropboxAuthenticationHandler([NotNull] HttpClient client)
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

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, DropboxAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, DropboxAuthenticationHelper.GetDisplayName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Email, DropboxAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer);

            // TODO: Add any optional claims, eg
            //  .AddOptionalClaim("urn:dropbox:name", DropboxAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)

            var context = new OAuthAuthenticatedContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.Authenticated(context);

            if (context.Principal?.Identity == null) {
                return null;
            }
                    
            return new AuthenticationTicket(context.Principal, context.Properties, context.Options.AuthenticationScheme);
        }
    }
}
