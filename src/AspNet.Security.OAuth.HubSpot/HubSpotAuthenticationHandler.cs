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
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.HubSpot;

public partial class HubSpotAuthenticationHandler : OAuthHandler<HubSpotAuthenticationOptions>
{
    public HubSpotAuthenticationHandler(
        [NotNull] IOptionsMonitor<HubSpotAuthenticationOptions> options,
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
        using JsonDocument userProfile = await GetUserProfileAsync(tokens);
        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, userProfile.RootElement);
        context.RunClaimActions();
        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    private async Task<JsonDocument> GetUserProfileAsync(
        [NotNull] OAuthTokenResponse tokens)
    {
        // Get the meta data for an access or refresh token.This
        // can be used to get the email address of the HubSpot user
        // that the token was created for, as well as the Hub ID that the token is associated with.
        // https://developers.hubspot.com/docs/api/oauth/tokens
        var accessToken = HttpUtility.UrlEncode(tokens.AccessToken);
        var url = string.Format(CultureInfo.InvariantCulture, Options.UserInformationEndpoint, accessToken);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        return JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
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

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
