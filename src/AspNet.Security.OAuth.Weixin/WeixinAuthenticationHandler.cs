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

namespace AspNet.Security.OAuth.Weixin;

public partial class WeixinAuthenticationHandler : OAuthHandler<WeixinAuthenticationOptions>
{
    public WeixinAuthenticationHandler(
        [NotNull] IOptionsMonitor<WeixinAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder,
        [NotNull] ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    private const string OauthState = "_oauthstate";
    private const string State = "state";

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        if (!IsWeixinAuthorizationEndpointInUse())
        {
            if (Request.Query.TryGetValue(OauthState, out var stateValue))
            {
                var query = Request.Query.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
                if (query.TryGetValue(State, out _))
                {
                    query[State] = stateValue;
                    Request.QueryString = QueryString.Create(query);
                }
            }
        }

        return await base.HandleRemoteAuthenticateAsync();
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string?>
        {
            ["access_token"] = tokens.AccessToken,
            ["openid"] = tokens.Response?.RootElement.GetString("openid")
        });

        using var response = await Backchannel.GetAsync(address);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        var json = await response.Content.ReadAsStringAsync(Context.RequestAborted);
        using var payload = JsonDocument.Parse(json);

        var errorCode = payload.RootElement.GetString("errcode");
        if (!string.IsNullOrEmpty(errorCode))
        {
            Log.UserProfileErrorCode(Logger, errorCode, response.Headers.ToString(), json);
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
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["appid"] = Options.ClientId,
            ["secret"] = Options.ClientSecret,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code"
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, tokenRequestParameters);

        using var response = await Backchannel.GetAsync(address);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        var json = await response.Content.ReadAsStringAsync(Context.RequestAborted);
        var payload = JsonDocument.Parse(json);

        var errorCode = payload.RootElement.GetString("errcode");
        if (!string.IsNullOrEmpty(errorCode))
        {
            Log.ExchangeCodeErrorCode(Logger, errorCode, response.Headers.ToString(), json);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        return OAuthTokenResponse.Success(payload);
    }

    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var scopeParameter = properties.GetParameter<ICollection<string>>(OAuthChallengeProperties.ScopeKey);
        var scope = scopeParameter != null ? FormatScope(scopeParameter) : FormatScope();

        var parameters = new Dictionary<string, string?>()
        {
            ["appid"] = Options.ClientId,
            ["scope"] = scope,
            ["response_type"] = "code",
        };

        if (Options.UsePkce)
        {
            var bytes = RandomNumberGenerator.GetBytes(256 / 8);
            var codeVerifier = WebEncoders.Base64UrlEncode(bytes);

            // Store this for use during the code redemption.
            properties.Items.Add(OAuthConstants.CodeVerifierKey, codeVerifier);

            var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

            parameters[OAuthConstants.CodeChallengeKey] = WebEncoders.Base64UrlEncode(challengeBytes);
            parameters[OAuthConstants.CodeChallengeMethodKey] = OAuthConstants.CodeChallengeMethodS256;
        }

        string state = Options.StateDataFormat.Protect(properties);
        bool addRedirectHash = false;

        if (!IsWeixinAuthorizationEndpointInUse())
        {
            // Store state in redirectUri when authorizing Wechat Web pages to prevent "too long state parameters" error
            redirectUri = QueryHelpers.AddQueryString(redirectUri, OauthState, state);
            addRedirectHash = true;
        }

        parameters["redirect_uri"] = redirectUri;
        parameters[State] = addRedirectHash ? OauthState : state;

        string challengeUrl = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);

        if (addRedirectHash)
        {
            // The parameters necessary for Web Authorization of Wechat
            challengeUrl += "#wechat_redirect";
        }

        return challengeUrl;
    }

    /// <inheritdoc/>
    protected override string FormatScope()
        => FormatScope(Options.Scope); // TODO This override is the same as the base class' and can be removed in the next major version

    /// <inheritdoc/>
    protected override string FormatScope([NotNull] IEnumerable<string> scopes)
        => string.Join(',', scopes);

    private bool IsWeixinAuthorizationEndpointInUse()
    {
        return string.Equals(Options.AuthorizationEndpoint, WeixinAuthenticationDefaults.AuthorizationEndpoint, StringComparison.OrdinalIgnoreCase);
    }

    private static partial class Log
    {
        internal static async Task ExchangeCodeErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ExchangeCodeError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving an access token with error {ErrorCode}: {Headers} {Body}.")]
        internal static partial void ExchangeCodeErrorCode(
            ILogger logger,
            string errorCode,
            string headers,
            string body);

        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserProfileError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(4, LogLevel.Error, "An error occurred while retrieving retrieving the user profile with error {ErrorCode}: {Headers} {Body}.")]
        internal static partial void UserProfileErrorCode(
            ILogger logger,
            string errorCode,
            string headers,
            string body);

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(3, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
