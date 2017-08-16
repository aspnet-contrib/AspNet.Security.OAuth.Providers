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

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// A class representing an authentication handler for Patreon.
    /// </summary>
    public class PatreonAuthenticationHandler : OAuthHandler<PatreonAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatreonAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to use.</param>
        public PatreonAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity,
            AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
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

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());


            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, PatreonAuthenticationHelper.GetUserIdentifier(payload),
                    Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, PatreonAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Webpage, PatreonAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:patreon:avatar", PatreonAuthenticationHelper.GetAvatarLink(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
