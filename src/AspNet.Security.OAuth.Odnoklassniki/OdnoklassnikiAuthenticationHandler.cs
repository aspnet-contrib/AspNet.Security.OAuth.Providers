/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Odnoklassniki;

public partial class OdnoklassnikiAuthenticationHandler : OAuthHandler<OdnoklassnikiAuthenticationOptions>
{
    public OdnoklassnikiAuthenticationHandler(
        [NotNull] IOptionsMonitor<OdnoklassnikiAuthenticationOptions> options,
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
        var accessSecret = GetMD5Hash(tokens.AccessToken + Options.ClientSecret);
        var sign = GetMD5Hash($"application_key={Options.PublicSecret}format=jsonmethod=users.getCurrentUser{accessSecret}");

        var parameters = new Dictionary<string, string?>
        {
            ["application_key"] = Options.PublicSecret ?? string.Empty,
            ["format"] = "json",
            ["method"] = "users.getCurrentUser",
            ["sig"] = sign,
            ["access_token"] = tokens.AccessToken,
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        using var request = new HttpRequestMessage(HttpMethod.Get, address);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override string FormatScope([NotNull] IEnumerable<string> scopes)
        => string.Join(';', scopes);

    private static string GetMD5Hash(string input)
    {
#pragma warning disable CA5351
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
#pragma warning restore CA5351

        return Convert.ToHexString(hash).ToLowerInvariant();
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
