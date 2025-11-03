/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Net.Http.Headers;
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
        // First, get the basic user info and shop_id from /v3/application/users/me
        using var meRequest = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        meRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

        // Add the basic claims from the /me endpoint
        // Use shop_id as the primary identifier for Etsy (required for most API operations)
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, shopId.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.String, Options.ClaimsIssuer));
        identity.AddClaim(new Claim(EtsyAuthenticationConstants.Claims.UserId, userId.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.String, Options.ClaimsIssuer));
        identity.AddClaim(new Claim(EtsyAuthenticationConstants.Claims.ShopId, shopId.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.String, Options.ClaimsIssuer));

        // Now get additional user details from /v3/application/users/{user_id}
        var userDetailEndpoint = $"https://openapi.etsy.com/v3/application/users/{userId}";
        using var userRequest = new HttpRequestMessage(HttpMethod.Get, userDetailEndpoint);
        userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        userRequest.Headers.Add("x-api-key", Options.ClientId);

        using var userResponse = await Backchannel.SendAsync(userRequest, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (userResponse.IsSuccessStatusCode)
        {
            using var userPayload = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync(Context.RequestAborted));

            // Create context with the detailed user data for claim mapping
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, userPayload.RootElement);
            context.RunClaimActions();

            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }
        else
        {
            // If detailed user info call fails, just create ticket with basic info
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, meRoot);

            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }
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
