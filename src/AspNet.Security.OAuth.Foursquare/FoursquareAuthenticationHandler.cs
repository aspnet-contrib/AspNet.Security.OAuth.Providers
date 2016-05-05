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
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Foursquare {
    public class FoursquareAuthenticationHandler : OAuthHandler<FoursquareAuthenticationOptions> {
        public FoursquareAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            // See https://developer.foursquare.com/overview/versioning
            // for more information about the mandatory "v" and "m" parameters.
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string> {
                ["m"] = "foursquare",
                ["v"] = Options.ApiVersion,
                ["oauth_token"] = tokens.AccessToken,
            });

            var request = new HttpRequestMessage(HttpMethod.Get, address);      
      
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
           
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, FoursquareAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, FoursquareAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, FoursquareAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, FoursquareAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, FoursquareAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, FoursquareAuthenticationHelper.GetContactEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Uri, FoursquareAuthenticationHelper.GetCanonicalUrl(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }              
    }
}
