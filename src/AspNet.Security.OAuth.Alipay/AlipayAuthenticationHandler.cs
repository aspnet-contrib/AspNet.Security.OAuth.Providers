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

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// Defines a handler for authentication using Alipay.
/// </summary>
public partial class AlipayAuthenticationHandler : OAuthHandler<AlipayAuthenticationOptions>
{
    public AlipayAuthenticationHandler(
        [NotNull] IOptionsMonitor<AlipayAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    private const string AuthCode = "auth_code";

    protected override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        if (TryStandardizeRemoteAuthenticateQuery(Request.Query, out var queryString))
        {
            Request.QueryString = queryString;
        }

        return base.HandleRemoteAuthenticateAsync();
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://opendocs.alipay.com/apis/api_9/alipay.system.oauth.token for details.
        var tokenRequestParameters = new SortedDictionary<string, string?>()
        {
            ["app_id"] = Options.ClientId,
            ["charset"] = "utf-8",
            ["code"] = context.Code,
            ["format"] = "JSON",
            ["grant_type"] = "authorization_code",
            ["method"] = "alipay.system.oauth.token",
            ["sign_type"] = "RSA2",
            ["timestamp"] = TimeProvider.GetUtcNow().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            ["version"] = "1.0",
        };
        tokenRequestParameters.Add("sign", GetRSA2Signature(tokenRequestParameters));

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, tokenRequestParameters);

        using var response = await Backchannel.GetAsync(address, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.AccessTokenError(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var document = await JsonDocument.ParseAsync(stream);

        var mainElement = document.RootElement.GetProperty("alipay_system_oauth_token_response");
        if (!ValidateReturnCode(mainElement, out var code))
        {
            return OAuthTokenResponse.Failed(new Exception($"An error (Code:{code}) occurred while retrieving an access token."));
        }

        var payload = JsonDocument.Parse(mainElement.GetRawText());
        return OAuthTokenResponse.Success(payload);
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        // See https://opendocs.alipay.com/apis/api_2/alipay.user.info.share for details.
        var parameters = new SortedDictionary<string, string?>()
        {
            ["app_id"] = Options.ClientId,
            ["auth_token"] = tokens.AccessToken,
            ["charset"] = "utf-8",
            ["format"] = "JSON",
            ["method"] = "alipay.user.info.share",
            ["sign_type"] = "RSA2",
            ["timestamp"] = TimeProvider.GetUtcNow().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            ["version"] = "1.0",
        };
        parameters.Add("sign", GetRSA2Signature(parameters));

        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var document = await JsonDocument.ParseAsync(stream);
        var rootElement = document.RootElement;

        if (!rootElement.TryGetProperty("alipay_user_info_share_response", out JsonElement mainElement))
        {
            var errorCode = rootElement.GetProperty("error_response").GetProperty("code").GetString()!;
            throw new AuthenticationFailureException($"An error (Code:{errorCode}) occurred while retrieving user information.");
        }

        if (!ValidateReturnCode(mainElement, out var code))
        {
            throw new AuthenticationFailureException($"An error (Code:{code}) occurred while retrieving user information.");
        }

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, mainElement.GetString("user_id")!, ClaimValueTypes.String, Options.ClaimsIssuer));

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
    /// <param name="element">Main part of json document from response</param>
    /// <param name="code">Returned code from server</param>
    /// <remarks>See https://opendocs.alipay.com/open/common/105806 for details.</remarks>
    /// <returns>True if succeed, otherwise false.</returns>
    private static bool ValidateReturnCode(JsonElement element, out string code)
    {
        if (!element.TryGetProperty("code", out JsonElement codeElement))
        {
            code = string.Empty;
            return true;
        }

        code = codeElement.GetString()!;
        return code == "10000";
    }

    /// <summary>
    /// Gets the RSA2(SHA256 with RSA) signature.
    /// </summary>
    /// <param name="sortedPairs">Sorted key-value pairs</param>
    private string GetRSA2Signature([NotNull] SortedDictionary<string, string?> sortedPairs)
    {
        var builder = new StringBuilder(128);

        foreach (var pair in sortedPairs)
        {
            if (string.IsNullOrEmpty(pair.Value))
            {
                continue;
            }

            builder.Append(pair.Key)
                   .Append('=')
                   .Append(pair.Value)
                   .Append('&');
        }

        var plainText = builder.ToString();
        var plainBytes = Encoding.UTF8.GetBytes(plainText, 0, plainText.Length - 1); // Skip the last '&'
        var privateKeyBytes = Convert.FromBase64String(Options.ClientSecret);

        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

        var encryptedBytes = rsa.SignData(plainBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return Convert.ToBase64String(encryptedBytes);
    }

    /// <inheritdoc />
    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var scopeParameter = properties.GetParameter<ICollection<string>>(OAuthChallengeProperties.ScopeKey);
        var scope = scopeParameter != null ? FormatScope(scopeParameter) : FormatScope();

        var parameters = new Dictionary<string, string?>
        {
            ["app_id"] = Options.ClientId, // Used instead of "client_id"
            ["scope"] = scope,
            ["response_type"] = "code",
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
            parameters[OAuthConstants.CodeChallengeKey] = WebEncoders.Base64UrlEncode(challengeBytes);
            parameters[OAuthConstants.CodeChallengeMethodKey] = OAuthConstants.CodeChallengeMethodS256;
        }

        parameters["state"] = Options.StateDataFormat.Protect(properties);

        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
    }

    private static bool TryStandardizeRemoteAuthenticateQuery(IQueryCollection query, out QueryString queryString)
    {
        if (!query.TryGetValue(AuthCode, out var authCode))
        {
            queryString = default;
            return false;
        }

        // Before: mydomain/signin-alipay?auth_code=xxx&state=xxx&...
        // After: mydomain/signin-alipay?code=xxx&state=xxx&...
        var queryParams = new List<KeyValuePair<string, StringValues>>(query.Count)
        {
            new("code", authCode)
        };
        foreach (var item in query)
        {
            switch (item.Key)
            {
                case "code":
                case AuthCode: // No need in fact, skip it
                    break;

                default:
                    queryParams.Add(item);
                    break;
            }
        }

        queryString = QueryString.Create(queryParams);
        return true;
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
