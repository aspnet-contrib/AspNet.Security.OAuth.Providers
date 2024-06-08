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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Smartsheet;

public partial class SmartsheetAuthenticationHandler : OAuthHandler<SmartsheetAuthenticationOptions>
{
    public SmartsheetAuthenticationHandler(
        [NotNull] IOptionsMonitor<SmartsheetAuthenticationOptions> options,
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
        using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = await JsonDocument.ParseAsync(stream, cancellationToken: Context.RequestAborted);

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // Compared to standard OAuth implementations, Smartsheet has an extra step at this point—creating an SHA-256
        // hash that is passed back with the access token request. The hash contains the app secret and authorization
        // code separated by a pipe, "app_secret|auth_code".
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{Options.ClientSecret}|{context.Code}")));

        var tokenRequestParameters = new Dictionary<string, string>()
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = Options.ClientId,
            ["code"] = context.Code,
            ["hash"] = hash
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new FormUrlEncodedContent(tokenRequestParameters);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        var payload = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(Context.RequestAborted), cancellationToken: Context.RequestAborted);

        return OAuthTokenResponse.Success(payload);
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

        internal static async Task ExchangeCodeErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ExchangeCodeError(
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

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
