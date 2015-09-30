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
using Microsoft.AspNet.Http.Extensions;

namespace AspNet.Security.OAuth.Foursquare {
    public class FoursquareAuthenticationHandler : OAuthHandler<FoursquareAuthenticationOptions> {
        public FoursquareAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {


            var queryBuilder = new QueryBuilder() {
                { "oauth_token", tokens.AccessToken },
                // For "v" and "m" query parameter, please refer to "https://developer.foursquare.com/overview/versioning". 
                {"v", Options.ApiVersion },
                // An m parameter, which specifies response mode style i.e. Foursquare-style responses. The "m" parameter is now mandatory.
                {"m", "foursquare" }
            };

            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint + queryBuilder.ToQueryString());            
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, FoursquareAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, FoursquareAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, FoursquareAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, FoursquareAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, FoursquareAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, FoursquareAuthenticationHelper.GetContactEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Uri, FoursquareAuthenticationHelper.GetCanonicalUrl(payload), Options.ClaimsIssuer);            
            
           await Options.Events.CreatingTicket(context);

           if (context.Principal?.Identity == null) {
                return null;
            }
                    
            return new AuthenticationTicket(context.Principal, context.Properties, context.Options.AuthenticationScheme);
        }              
    }
}
