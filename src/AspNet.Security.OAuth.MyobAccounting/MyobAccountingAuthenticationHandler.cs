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
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.MyobAccounting {
    public class MyobAccountingAuthenticationHandler : OAuthHandler<MyobAccountingAuthenticationOptions> {
        public MyobAccountingAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {


            //Myob doesn't provide a user information end point, so we rely on the details sent back in the token request. 
            var user = new JObject();
            foreach (var prop in tokens.Response.SelectToken("user")) {
                user.Add(prop);
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, MyobAccountingAuthenticationHelper.GetIdentifier(user), 
                Options.ClaimsIssuer);

            identity.AddOptionalClaim(ClaimTypes.Name, MyobAccountingAuthenticationHelper.GetUsername(user),
                Options.ClaimsIssuer);
            
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, user);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
