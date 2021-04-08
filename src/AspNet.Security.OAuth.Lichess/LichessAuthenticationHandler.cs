/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Lichess
{
    /// <summary>
    /// Defines a handler for authentication using Lichess.
    /// </summary>
    public class LichessAuthenticationHandler : OAuthHandler<LichessAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LichessAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        public LichessAuthenticationHandler(
            [NotNull] IOptionsMonitor<LichessAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            // Retrieve basic user information
            using var payload = await RequestUserInformationAsync(Options.UserInformationEndpoint, tokens.AccessToken!, "user profile");

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            // Now retrieve email address if scope is added
            if (!string.IsNullOrEmpty(Options.UserEmailsEndpoint) &&
                !identity.HasClaim(claim => claim.Type == ClaimTypes.Email) &&
                Options.Scope.Contains(LichessAuthenticationConstants.Scopes.EmailRead))
            {
                using var emailPayload = await RequestUserInformationAsync(Options.UserEmailsEndpoint, tokens.AccessToken!, "email address");

                context.RunClaimActions(emailPayload.RootElement);
            }

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        /// <summary>
        /// Performs a backchannel request to obtain user information from the specified endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint to return information from</param>
        /// <param name="accessToken">Bearer token for authentication</param>
        /// <param name="requestInformationType">Descriptive text of type of information request in event of exception</param>
        /// <returns>Parsed JSON document response from endpoint</returns>
        private async Task<JsonDocument> RequestUserInformationAsync(string endpoint, string accessToken, string requestInformationType)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"An error occurred while retrieving the {requestInformationType}: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException($"An error occurred while retrieving the {requestInformationType}.");
            }

            return JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        }
    }
}
