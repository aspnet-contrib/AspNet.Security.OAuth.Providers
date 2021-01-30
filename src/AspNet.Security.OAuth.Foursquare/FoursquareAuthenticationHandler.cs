/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Foursquare
{
    public class FoursquareAuthenticationHandler : OAuthHandler<FoursquareAuthenticationOptions>
    {
        public FoursquareAuthenticationHandler(
            [NotNull] IOptionsMonitor<FoursquareAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            // See https://developer.foursquare.com/overview/versioning
            // for more information about the mandatory "v" and "m" parameters.
            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string?>
            {
                ["m"] = "foursquare",
                ["v"] = !string.IsNullOrEmpty(Options.ApiVersion) ? Options.ApiVersion : FoursquareAuthenticationDefaults.ApiVersion,
                ["oauth_token"] = tokens.AccessToken,
            });

            using var request = new HttpRequestMessage(HttpMethod.Get, address);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);

            if (payload.RootElement.TryGetProperty("response", out var responseProperty))
            {
                context.RunClaimActions(responseProperty.GetProperty("user"));
            }
            else
            {
                context.RunClaimActions();
            }

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }
    }
}
