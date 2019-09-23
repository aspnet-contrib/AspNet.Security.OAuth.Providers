/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace AspNet.Security.OAuth.ArcGIS
{
    public class ArcGISAuthenticationHandler : OAuthHandler<ArcGISAuthenticationOptions>
    {
        public ArcGISAuthenticationHandler(
            [NotNull] IOptionsMonitor<ArcGISAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            // Note: the ArcGIS API doesn't support content negotiation via headers.
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["f"] = "json",
                ["token"] = tokens.AccessToken
            });

            using var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Request the token
            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            // Note: error responses always return 200 status codes.
            if (payload.RootElement.TryGetProperty("error", out var error))
            {
                // See https://developers.arcgis.com/authentication/server-based-user-logins/ for more information
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a response with the following error code: {Code} {Message}.",
                                /* Code: */ error.GetString("code"),
                                /* Message: */ error.GetString("message"));

                throw new InvalidOperationException("An error occurred while retrieving the user profile.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
