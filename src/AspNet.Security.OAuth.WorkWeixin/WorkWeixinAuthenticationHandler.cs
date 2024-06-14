/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.WorkWeixin;

public partial class WorkWeixinAuthenticationHandler : OAuthHandler<WorkWeixinAuthenticationOptions>
{
    public WorkWeixinAuthenticationHandler(
        [NotNull] IOptionsMonitor<WorkWeixinAuthenticationOptions> options,
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
        (var errorCode, var userId) = await GetUserIdentifierAsync(tokens);
        if (errorCode != 0 || string.IsNullOrEmpty(userId))
        {
            throw new AuthenticationFailureException($"An error (Code:{errorCode}) occurred while retrieving the user identifier.");
        }

        // See https://open.work.weixin.qq.com/api/doc/90000/90135/90196 for details.
        var parameters = new Dictionary<string, string?>
        {
            ["access_token"] = tokens.AccessToken,
            ["userid"] = userId,
        };

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        errorCode = payload.RootElement.GetProperty("errcode").GetInt32();
        if (errorCode != 0)
        {
            Log.UserProfileErrorCode(Logger, errorCode, payload.RootElement.GetString("errmsg"));
            throw new AuthenticationFailureException("An error occurred while retrieving user information.");
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://open.work.weixin.qq.com/api/doc/90000/90135/91039 for details.
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["corpid"] = Options.ClientId,
            ["corpsecret"] = Options.ClientSecret,
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, tokenRequestParameters);

        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        return OAuthTokenResponse.Success(payload);
    }

    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var parameters = new Dictionary<string, string?>
        {
            ["appid"] = Options.ClientId,
            ["agentid"] = Options.AgentId,
            ["redirect_uri"] = redirectUri,
        };

        foreach (var additionalParameter in Options.AdditionalAuthorizationParameters)
        {
            parameters.Add(additionalParameter.Key, additionalParameter.Value);
        }

        if (Options.UsePkce)
        {
            var bytes = RandomNumberGenerator.GetBytes(256 / 8);
            var codeVerifier = WebEncoders.Base64UrlEncode(bytes);

            // Store this for use during the code redemption.
            properties.Items.Add(OAuthConstants.CodeVerifierKey, codeVerifier);

            var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
            var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);

            parameters[OAuthConstants.CodeChallengeKey] = codeChallenge;
            parameters[OAuthConstants.CodeChallengeMethodKey] = OAuthConstants.CodeChallengeMethodS256;
        }

        parameters["state"] = Options.StateDataFormat.Protect(properties);

        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
    }

    private async Task<(int ErrorCode, string? UserId)> GetUserIdentifierAsync(OAuthTokenResponse tokens)
    {
        // See https://open.work.weixin.qq.com/api/doc/90000/90135/91023 for details.
        var parameters = new Dictionary<string, string?>
        {
            ["access_token"] = tokens.AccessToken,
            ["code"] = Request.Query["code"],
        };

        var address = QueryHelpers.AddQueryString(Options.UserIdentificationEndpoint, parameters);

        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserIdentifierErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user identifier.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        var errorCode =
            payload.RootElement.TryGetProperty("errcode", out var errCodeElement) && errCodeElement.ValueKind == JsonValueKind.Number ?
            errCodeElement.GetInt32() :
            0;

        return (errorCode, payload.RootElement.GetString("UserId"));
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

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {ErrorCode} response with the following message: {ErrorMessage}.")]
        internal static partial void UserProfileErrorCode(
            ILogger logger,
            int errorCode,
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
