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

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        // Get the basic user info (user_id and shop_id)
        using var meRequest = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        meRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        meRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        meRequest.Headers.Add("x-api-key", Options.ClientId);

        using var meResponse = await Backchannel.SendAsync(meRequest, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!meResponse.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, meResponse, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving basic user info from Etsy.");
        }

        using var mePayload = JsonDocument.Parse(await meResponse.Content.ReadAsStringAsync(Context.RequestAborted));
        var meRoot = mePayload.RootElement;

        // Extract user_id and shop_id from the /me response
        // Both fields should always be present in a successful Etsy OAuth response
        var userId = meRoot.GetProperty("user_id").GetInt64();
        var shopId = meRoot.GetProperty("shop_id").GetInt64();

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, meRoot);

        // Map claims from the basic payload first
        context.RunClaimActions();

        // Optionally enrich with detailed user info
        if (Options.IncludeDetailedUserInfo)
        {
            using var detailedPayload = await GetDetailedUserInfoAsync(tokens);
            context.RunClaimActions(detailedPayload.RootElement);
        }

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <summary>
    /// Retrieves detailed user information from Etsy.
    /// </summary>
    /// <param name="tokens">The OAuth token response.</param>
    /// <returns>A JSON document containing the detailed user information.</returns>
    protected virtual async Task<JsonDocument> GetDetailedUserInfoAsync([NotNull] OAuthTokenResponse tokens)
    {
        using var userRequest = new HttpRequestMessage(HttpMethod.Get, EtsyAuthenticationDefaults.EtsyBaseUri + EtsyAuthenticationDefaults.UserDetailsPath);
        userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        userRequest.Headers.Add("x-api-key", Options.ClientId);

        using var userResponse = await Backchannel.SendAsync(userRequest, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!userResponse.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, userResponse, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving detailed user info from Etsy.");
        }

        return JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync(Context.RequestAborted));
    }

    private static partial class Log
    {
        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserProfileError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile from Etsy: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
