/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Zalo;

public partial class ZaloAuthenticationHandler : OAuthHandler<ZaloAuthenticationOptions>
{
    public ZaloAuthenticationHandler(
        [NotNull] IOptionsMonitor<ZaloAuthenticationOptions> options,
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
        // See https://developers.zalo.me/docs/api/social-api/tai-lieu/thong-tin-nguoi-dung-post-28
        var parameters = new Dictionary<string, string?>
        {
            ["access_token"] = tokens.AccessToken,
            ["fields"] = "id,name,birthday,gender",
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        using var request = new HttpRequestMessage(HttpMethod.Get, address);

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

    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var challengeUrl = base.BuildChallengeUrl(properties, redirectUri);
        return QueryHelpers.AddQueryString(challengeUrl, "app_id", Options.ClientId);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        var tokenRequestParameters = new Dictionary<string, string?>
        {
            ["app_id"] = Options.ClientId,
            ["code"] = context.Code,
            ["redirect_uri"] = context.RedirectUri,
            ["grant_type"] = "authorization_code"
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(tokenRequestParameters)
        };

        Backchannel.DefaultRequestHeaders.Add("secret_key", Options.ClientSecret);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

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
            HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
