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

namespace AspNet.Security.OAuth.QQ;

public partial class QQAuthenticationHandler : OAuthHandler<QQAuthenticationOptions>
{
    public QQAuthenticationHandler(
        [NotNull] IOptionsMonitor<QQAuthenticationOptions> options,
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
        (var errorCode, var openId, var unionId) = await GetUserIdentifierAsync(tokens);

        if (errorCode != 0 || string.IsNullOrEmpty(openId))
        {
            throw new HttpRequestException($"An error (Code:{errorCode}) occurred while retrieving the user identifier.");
        }

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, openId, ClaimValueTypes.String, Options.ClaimsIssuer));
        if (!string.IsNullOrEmpty(unionId))
        {
            identity.AddClaim(new Claim(QQAuthenticationConstants.Claims.UnionId, unionId, ClaimValueTypes.String, Options.ClaimsIssuer));
        }

        var parameters = new Dictionary<string, string?>(3)
        {
            ["oauth_consumer_key"] = Options.ClientId,
            ["access_token"] = tokens.AccessToken,
            ["openid"] = openId,
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        using var response = await Backchannel.GetAsync(address);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = JsonDocument.Parse(stream);

        var status = payload.RootElement.GetProperty("ret").GetInt32();

        if (status != 0)
        {
            Log.UserProfileErrorCode(Logger, status, payload.RootElement.GetString("msg"));
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://wiki.connect.qq.com/%E4%BD%BF%E7%94%A8authorization_code%E8%8E%B7%E5%8F%96access_token for details
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret,
            ["redirect_uri"] = context.RedirectUri,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code",
            ["fmt"] = "json", // Return JSON instead of x-www-form-urlencoded which is default due to historical reasons
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, tokenRequestParameters);

        using var request = new HttpRequestMessage(HttpMethod.Get, address);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        var payload = JsonDocument.Parse(stream);

        return OAuthTokenResponse.Success(payload);
    }

    private async Task<(int ErrorCode, string? OpenId, string? UnionId)> GetUserIdentifierAsync(OAuthTokenResponse tokens)
    {
        // See https://wiki.connect.qq.com/unionid%E4%BB%8B%E7%BB%8D for details
        var queryString = new Dictionary<string, string?>(3)
        {
            ["access_token"] = tokens.AccessToken,
            ["fmt"] = "json" // Return JSON instead of JSONP which is default due to historical reasons
        };

        if (Options.ApplyForUnionId)
        {
            queryString.Add("unionid", "1");
        }

        var address = QueryHelpers.AddQueryString(Options.UserIdentificationEndpoint, queryString);
        using var request = new HttpRequestMessage(HttpMethod.Get, address);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserIdentifierErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user identifier.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using JsonDocument payload = JsonDocument.Parse(stream);

        var payloadRoot = payload.RootElement;

        var errorCode =
            payloadRoot.TryGetProperty("error", out var errorCodeElement) && errorCodeElement.ValueKind == JsonValueKind.Number ?
            errorCodeElement.GetInt32() :
            0;

        return (errorCode, payloadRoot.GetString("openid"), payloadRoot.GetString("unionid"));
    }

    protected override string FormatScope([NotNull] IEnumerable<string> scopes)
        => string.Join(',', scopes);

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

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following message: {ErrorMessage}.")]
        internal static partial void UserProfileErrorCode(
            ILogger logger,
            int status,
            string? errorMessage);

        internal static async Task ExchangeCodeErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ExchangeCodeError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static async Task UserIdentifierErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserIdentifierError(
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

        [LoggerMessage(3, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(4, LogLevel.Warning, "An error occurred while retrieving the user identifier: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserIdentifierError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
