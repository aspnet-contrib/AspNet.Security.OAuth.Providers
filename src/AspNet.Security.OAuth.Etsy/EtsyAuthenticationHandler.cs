/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Etsy;

public partial class EtsyAuthenticationHandler : OAuthHandler<EtsyAuthenticationOptions>
{
    public EtsyAuthenticationHandler(
        [NotNull] IOptionsMonitor<EtsyAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <summary>
    /// Creates an <see cref="AuthenticationTicket"/> from the OAuth tokens and Etsy user information.
    /// </summary>
    /// <param name="identity">The claims identity to populate.</param>
    /// <param name="properties">The authentication properties.</param>
    /// <param name="tokens">The OAuth token response containing the access token.</param>
    /// <returns>An <see cref="AuthenticationTicket"/> containing the user claims and properties.</returns>
    /// <exception cref="HttpRequestException">Thrown when an error occurs while retrieving user information from Etsy.</exception>
    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        // Get the basic user info (user_id and shop_id)
        using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        request.Headers.Add("x-api-key", Options.ClientId);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.BasicUserInfoErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving basic user information from Etsy.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        var meRoot = payload.RootElement;

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, meRoot);

        // Map claims from the basic payload first
        context.RunClaimActions();

        // Optionally enrich with detailed user info if requested
        if (Options.IncludeDetailedUserInfo)
        {
            // Extract user_id from the /me response
            var userId = meRoot.GetProperty("user_id").GetInt64();

            using var detailedPayload = await GetDetailedUserInfoAsync(tokens, userId);
            var detailedRoot = detailedPayload.RootElement;

            // Apply claim actions for fields that are only in the detailed payload
            // We filter the ClaimActions to exclude those for user_id and shop_id
            // since they were already processed from the basic /users/me endpoint
            foreach (var action in Options.ClaimActions)
            {
                // Skip the action if it's a JsonKeyClaimAction for user_id or shop_id
                if (action is Microsoft.AspNetCore.Authentication.OAuth.Claims.JsonKeyClaimAction { ClaimType: var t } &&
                                    (t == ClaimTypes.NameIdentifier
                                  || t == EtsyAuthenticationConstants.Claims.ShopId))
                {
                    continue;
                }

                action.Run(detailedRoot, identity, Options.ClaimsIssuer ?? ClaimsIssuer);
            }
        }

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <summary>
    /// Retrieves detailed user information from Etsy.
    /// </summary>
    /// <param name="tokens">The OAuth token response.</param>
    /// <param name="userId">The user ID to retrieve details for.</param>
    /// <returns>A <see cref="JsonDocument"/> containing the detailed user information.</returns>
    protected virtual async Task<JsonDocument> GetDetailedUserInfoAsync([NotNull] OAuthTokenResponse tokens, long userId)
    {
        var userDetailsUrl = Options.DetailedUserInfoEndpoint.EndsWith('/') ? $"{Options.DetailedUserInfoEndpoint}{userId}" : $"{Options.DetailedUserInfoEndpoint}/{userId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, userDetailsUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        request.Headers.Add("x-api-key", Options.ClientId);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.DetailedUserInfoErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving detailed user info from Etsy.");
        }

        return JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
    }

    private static partial class Log
    {
        internal static async Task BasicUserInfoErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            BasicUserInfoError(
                logger,
                response.RequestMessage?.RequestUri?.ToString() ?? string.Empty,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static async Task DetailedUserInfoErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            DetailedUserInfoError(
                logger,
                response.RequestMessage?.RequestUri?.ToString() ?? string.Empty,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(1, LogLevel.Error, "Etsy basic user info request failed for '{RequestUri}': remote server returned a {Status} response with: {Headers} {Body}.")]
        private static partial void BasicUserInfoError(
            ILogger logger,
            string requestUri,
            System.Net.HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(2, LogLevel.Error, "Etsy detailed user info request failed for '{RequestUri}': remote server returned a {Status} response with: {Headers} {Body}.")]
        private static partial void DetailedUserInfoError(
            ILogger logger,
            string requestUri,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
