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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Slack
{
    public class SlackAuthenticationHandler : OAuthHandler<SlackAuthenticationOptions>
    {
        public SlackAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "token", tokens.AccessToken);

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, SlackAuthenticationHelper.GetUserIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, SlackAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:team_id", SlackAuthenticationHelper.GetTeamIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:team_name", SlackAuthenticationHelper.GetTeamName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:team_url", SlackAuthenticationHelper.GetTeamLink(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:bot:user_id", SlackAuthenticationHelper.GetBotUserId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:bot:access_token", SlackAuthenticationHelper.GetBotAccessToken(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:webhook:channel", SlackAuthenticationHelper.GetWebhookChannel(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:webhook:url", SlackAuthenticationHelper.GetWebhookURL(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:slack:webhook:configuration_url", SlackAuthenticationHelper.GetWebhookConfigurationURL(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
