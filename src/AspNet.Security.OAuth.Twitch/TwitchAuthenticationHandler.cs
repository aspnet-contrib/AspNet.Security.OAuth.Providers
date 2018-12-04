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
using static AspNet.Security.OAuth.Twitch.TwitchAuthenticationConstants;

namespace AspNet.Security.OAuth.Twitch
{
    public class TwitchAuthenticationHandler : OAuthHandler<TwitchAuthenticationOptions>
    {
        public TwitchAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
            => QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["scope"] = FormatScope(),
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties),
                ["force_verify"] = Options.ForceVerify ? "true" : "false"
            });

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
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

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, TwitchAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, TwitchAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.DisplayName, TwitchAuthenticationHelper.GetDisplayName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, TwitchAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.Type, TwitchAuthenticationHelper.GetType(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.BroadcasterType, TwitchAuthenticationHelper.GetBroadcastType(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.Description, TwitchAuthenticationHelper.GetDescription(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.ProfileImageUrl, TwitchAuthenticationHelper.GetProfileImageUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.OfflineImageUrl, TwitchAuthenticationHelper.GetOfflineImageUrl(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
