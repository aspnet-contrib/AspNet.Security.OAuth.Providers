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
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AspNet.Security.OAuth.ArcGIS {
    public class ArcGISAuthenticationHandler : OAuthHandler<ArcGISAuthenticationOptions> {
        public ArcGISAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {

            // The API doesn't support content negotiation via the header so we need to set the query param for the format (f for shorthand)
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string> { { "f", "json"}, { "token", tokens.AccessToken } });
            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
            // Request the token 
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var error = ArcGISAuthenticationHelper.GetError(payload); // error responses still return 200 status codes
            if (error != null) {
                Logger.LogError("An error occurred when retrieving the user information: the remote server " +
                    "returned a response with the following error code: {Status} {Description}.",
                    /* Status: */ error.Value<string>("code"),
                    /* Description: */ error.Value<string>("message"));
                return null;
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, ArcGISAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, ArcGISAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Email, ArcGISAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer);
            
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
