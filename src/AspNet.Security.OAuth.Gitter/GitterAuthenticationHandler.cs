﻿/*
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

namespace AspNet.Security.OAuth.Gitter
{
    public class GitterAuthenticationHandler : OAuthHandler<GitterAuthenticationOptions>
    {
        public GitterAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue(tokens.TokenType, tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JArray.Parse(await response.Content.ReadAsStringAsync());
            var user = (JObject) payload[0];

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, GitterAuthenticationHelper.GetIdentifier(user), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, GitterAuthenticationHelper.GetUsername(user), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Webpage, GitterAuthenticationHelper.GetLink(user), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:gitter:displayname", GitterAuthenticationHelper.GetDisplayName(user), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:gitter:avatarurlsmall", GitterAuthenticationHelper.GetAvatarUrlSmall(user), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:gitter:avatarurlmedium", GitterAuthenticationHelper.GetAvatarUrlMedium(user), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, user);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
