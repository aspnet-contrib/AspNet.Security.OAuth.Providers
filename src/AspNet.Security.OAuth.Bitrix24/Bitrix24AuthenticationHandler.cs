/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.Bitrix24
{
    /// <summary>
    /// Defines a handler for authentication using Bitrix24.
    /// </summary>
    public class Bitrix24AuthenticationHandler : OAuthHandler<Bitrix24AuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bitrix24AuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        public Bitrix24AuthenticationHandler(
            [NotNull] IOptionsMonitor<Bitrix24AuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override string BuildChallengeUrl(
            [NotNull] AuthenticationProperties properties,
            [NotNull] string redirectUri)
        {
            string challengeUrl = base.BuildChallengeUrl(properties, redirectUri);

            // Bitrix24 requires the response mode to be form_post when the email or name scopes are requested
            return QueryHelpers.AddQueryString(challengeUrl, "response_mode", "form_post");
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var authorizationHeader = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            Backchannel.DefaultRequestHeaders.Authorization = authorizationHeader;
            using var response = await Backchannel.GetAsync(Options.UserInformationEndpoint, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Bitrix24 user information ({response.StatusCode}). Please check if the authentication information is correct.");
            }

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted)))
            {
                var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement.GetProperty("result"));
                context.RunClaimActions();
                await Events.CreatingTicket(context);

                return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
            }
        }
    }
}
