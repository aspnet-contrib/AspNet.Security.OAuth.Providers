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

namespace AspNet.Security.OAuth.FourSquare {
    public class FourSquareAuthenticationHandler : OAuthHandler<FourSquareAuthenticationOptions> {
        public FourSquareAuthenticationHandler([NotNull] HttpClient client)
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
           
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, FourSquareAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, FourSquareAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, FourSquareAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, FourSquareAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, FourSquareAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, FourSquareAuthenticationHelper.GetContactEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Uri, FourSquareAuthenticationHelper.GetCanonicalUrl(payload), Options.ClaimsIssuer);            
            
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
