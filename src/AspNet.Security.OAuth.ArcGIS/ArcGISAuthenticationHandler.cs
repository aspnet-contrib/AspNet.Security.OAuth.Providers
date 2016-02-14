/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace AspNet.Security.OAuth.ArcGIS {
    public class ArcGISAuthenticationHandler : OAuthHandler<ArcGISAuthenticationOptions> {
        public ArcGISAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            // Note: the ArcGIS API doesn't support content negotiation via headers.
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string> {
                ["f"] = "json",
                ["token"] = tokens.AccessToken
            });

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
            // Request the token 
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Note: error responses always return 200 status codes.
            var error = ArcGISAuthenticationHelper.GetError(payload);
            if (error != null) {
                // See https://developers.arcgis.com/authentication/server-based-user-logins/ for more information
                Logger.LogError("An error occurred when retrieving the user information: the remote server " +
                                "returned a response with the following error code: {Code} {Message}.",
                                /* Code: */ error.Value<string>("code"),
                                /* Message: */ error.Value<string>("message"));

                return null;
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, ArcGISAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, ArcGISAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, ArcGISAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
