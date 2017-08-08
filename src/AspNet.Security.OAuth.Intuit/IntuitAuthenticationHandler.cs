/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Intuit
{
    public class IntuitAuthenticationHandler : OAuthHandler<IntuitAuthenticationOptions>
    {
        public IntuitAuthenticationHandler(HttpClient client)
            : base(client)
        {
        }
      

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity,
             AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var req = this.Request;
            var queryitems = req.Query;
            StringValues realmId;
            StringValues state;
            StringValues code;
            queryitems.TryGetValue("realmId", out realmId);
            queryitems.TryGetValue("state", out state);
            queryitems.TryGetValue("code", out code);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}