/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Vkontakte;

public partial class VkontakteAuthenticationHandler : OAuthHandler<VkontakteAuthenticationOptions>
{
    public VkontakteAuthenticationHandler(
        [NotNull] IOptionsMonitor<VkontakteAuthenticationOptions> options,
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
        var parameters = new Dictionary<string, string?>
        {
            ["access_token"] = tokens.AccessToken,
            ["v"] = !string.IsNullOrEmpty(Options.ApiVersion) ? Options.ApiVersion : VkontakteAuthenticationDefaults.ApiVersion,
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        if (Options.Fields.Count != 0)
        {
            address = QueryHelpers.AddQueryString(address, "fields", string.Join(',', Options.Fields));
        }

        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var container = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        if (!container.RootElement.TryGetProperty("response", out var profileResponse))
        {
            if (container.RootElement.TryGetProperty("error", out var error) &&
                error.ValueKind is JsonValueKind.String)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the user profile: {error.GetString()}");
            }

            throw new InvalidOperationException("An error occurred while retrieving the user profile.");
        }

        using var enumerator = profileResponse.EnumerateArray();
        var payload = enumerator.First();

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
        context.RunClaimActions();

        // Re-run to get the email claim from the tokens response
        context.RunClaimActions(tokens.Response!.RootElement);

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
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
