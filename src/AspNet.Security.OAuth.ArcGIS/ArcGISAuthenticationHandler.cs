/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.ArcGIS;

public partial class ArcGISAuthenticationHandler : OAuthHandler<ArcGISAuthenticationOptions>
{
    public ArcGISAuthenticationHandler(
        [NotNull] IOptionsMonitor<ArcGISAuthenticationOptions> options,
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
        // Note: the ArcGIS API doesn't support content negotiation via headers.
        var parameters = new Dictionary<string, string?>
        {
            ["f"] = "json",
            ["token"] = tokens.AccessToken,
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters!);

        using var request = new HttpRequestMessage(HttpMethod.Get, address);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Request the token
        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        // Note: error responses always return 200 status codes.
        if (payload.RootElement.TryGetProperty("error", out var error))
        {
            // See https://developers.arcgis.com/authentication/server-based-user-logins/ for more information
            Log.UserProfileError(Logger, error.GetString("code"), error.GetString("message"));

            throw new InvalidOperationException("An error occurred while retrieving the user profile.");
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a response with the following error code: {Code} {ErrorMessage}.")]
        internal static partial void UserProfileError(
            ILogger logger,
            string? code,
            string? errorMessage);
    }
}
