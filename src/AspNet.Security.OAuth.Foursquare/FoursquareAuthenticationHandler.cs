/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Foursquare {
    public class FoursquareAuthenticationHandler : OAuthHandler<FoursquareAuthenticationOptions> {
        public FoursquareAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string> {
                // See https://developer.foursquare.com/overview/versioning
                // for more information about the mandatory "v" and "m" parameters.
                ["m"] = "foursquare",
                ["v"] = Options.ApiVersion,
                ["oauth_token"] = tokens.AccessToken,
            });

            var request = new HttpRequestMessage(HttpMethod.Get, address);            
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
           
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, FoursquareAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, FoursquareAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, FoursquareAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, FoursquareAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, FoursquareAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, FoursquareAuthenticationHelper.GetContactEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Uri, FoursquareAuthenticationHelper.GetCanonicalUrl(payload), Options.ClaimsIssuer);            
            
            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null) {
                return null;
            }
                    
            return new AuthenticationTicket(context.Principal, context.Properties, context.Options.AuthenticationScheme);
        }              
    }
}
