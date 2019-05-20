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
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

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

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Request the token
            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Note: error responses always return 200 status codes.
            var error = payload.Value<JObject>("error");
            if (error != null)
            {
                // See https://developers.arcgis.com/authentication/server-based-user-logins/ for more information
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a response with the following error code: {Code} {Message}.",
                                /* Code: */ error.Value<string>("code"),
                                /* Message: */ error.Value<string>("message"));

                throw new InvalidOperationException("An error occurred while retrieving the user profile.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
