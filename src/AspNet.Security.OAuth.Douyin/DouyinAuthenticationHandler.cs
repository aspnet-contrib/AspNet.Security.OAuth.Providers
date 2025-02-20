/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
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
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.Douyin;

/// <summary>
/// Defines a handler for authentication using Douyin.
/// </summary>
public partial class DouyinAuthenticationHandler : OAuthHandler<DouyinAuthenticationOptions>
{
    public DouyinAuthenticationHandler(
        [NotNull] IOptionsMonitor<DouyinAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://developer.open-douyin.com/docs/resource/zh-CN/dop/develop/openapi/account-permission/get-access-token for details.
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["client_key"] = Options.ClientId,
            ["code"] = context.Code,
            ["client_secret"] = Options.ClientSecret,
            ["grant_type"] = "authorization_code",
        };

        using var tokenRequestContent = new FormUrlEncodedContent(tokenRequestParameters);

        using var response = await Backchannel.PostAsync(Options.TokenEndpoint, tokenRequestContent, Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            await Log.AccessTokenError(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var document = await JsonDocument.ParseAsync(stream);

        var mainElement = document.RootElement.GetProperty("data");
        if (!ValidateReturnCode(mainElement, out var errorCode))
        {
            return OAuthTokenResponse.Failed(new Exception($"An error (ErrorCode:{errorCode}) occurred while retrieving an access token."));
        }

        var payload = JsonDocument.Parse(mainElement.GetRawText());
        return OAuthTokenResponse.Success(payload);
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        // See https://developer.open-douyin.com/docs/resource/zh-CN/dop/develop/openapi/account-permission/get-account-open-info for details.
        var parameters = new SortedDictionary<string, string?>()
        {
            ["open_id"] = tokens.Response!.RootElement.GetProperty("open_id").GetString()!,
            ["access_token"] = tokens.AccessToken,
        };

        using var userInfoRequestContent = new FormUrlEncodedContent(parameters);

        using var response = await Backchannel.PostAsync(Options.UserInformationEndpoint, userInfoRequestContent, Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var document = await JsonDocument.ParseAsync(stream);
        var mainElement = document.RootElement.GetProperty("data");

        if (!ValidateReturnCode(mainElement, out var errorCode))
        {
            throw new AuthenticationFailureException($"An error (ErrorCode:{errorCode}) occurred while retrieving user information.");
        }

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, mainElement.GetString("open_id")!, ClaimValueTypes.String, Options.ClaimsIssuer));

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, mainElement);

        context.RunClaimActions();

        await Events.CreatingTicket(context);

        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override string FormatScope([NotNull] IEnumerable<string> scopes) => string.Join(',', Options.Scope);

    /// <summary>
    /// Check the code sent back by server for potential server errors.
    /// </summary>
    /// <param name="element">Main part of JSON document from response</param>
    /// <param name="errorCode">Returned error_code from server</param>
    /// <remarks>See https://developer.open-douyin.com/docs/resource/zh-CN/dop/develop/openapi/status-code for details.</remarks>
    /// <returns>True if succeed, otherwise false.</returns>
    private static bool ValidateReturnCode(JsonElement element, out int errorCode)
    {
        errorCode = 0;

        if (element.TryGetProperty("error_code", out JsonElement errorCodeElement))
        {
            errorCode = errorCodeElement.GetInt32()!;
        }

        return errorCode == 0;
    }

    /// <inheritdoc />
    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var scopeParameter = properties.GetParameter<ICollection<string>>(OAuthChallengeProperties.ScopeKey);
        var scope = scopeParameter != null ? FormatScope(scopeParameter) : FormatScope();

        var parameters = new Dictionary<string, string?>
        {
            ["client_key"] = Options.ClientId, // Used instead of "client_id"
            ["scope"] = scope,
            ["response_type"] = "code",
            ["redirect_uri"] = redirectUri,
        };

        foreach (var additionalParameter in Options.AdditionalAuthorizationParameters)
        {
            parameters.Add(additionalParameter.Key, additionalParameter.Value);
        }

        parameters["state"] = Options.StateDataFormat.Protect(properties);

        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
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

        internal static async Task AccessTokenError(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            AccessTokenError(
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
        private static partial void AccessTokenError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
