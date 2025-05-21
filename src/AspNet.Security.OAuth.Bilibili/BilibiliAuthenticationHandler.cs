/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Buffers.Text;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Bilibili;

public partial class BilibiliAuthenticationHandler : OAuthHandler<BilibiliAuthenticationOptions>
{
    public BilibiliAuthenticationHandler(
        [NotNull] IOptionsMonitor<BilibiliAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        var parameters = new Dictionary<string, string?>
        {
            ["client_id"] = Options.ClientId,
            ["response_type"] = "code",
            ["gourl"] = redirectUri // Used instead of "redirect_uri"
        };

        foreach (var additionalParameter in Options.AdditionalAuthorizationParameters)
        {
            parameters.Add(additionalParameter.Key, additionalParameter.Value);
        }

        parameters["state"] = Options.StateDataFormat.Protect(properties);

        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://open.bilibili.com/doc/4/eaf0e2b5-bde9-b9a0-9be1-019bb455701c#h1-u7B80u4ECB for details.
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["client_id"] = Options.ClientId,
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

        var mainElement = document.RootElement;

        if (!ValidateReturnCode(mainElement, out var code))
        {
            return OAuthTokenResponse.Failed(new Exception($"An error (Code:{code}) occurred while retrieving an access token."));
        }

        var payload = JsonDocument.Parse(mainElement.GetProperty("data").GetRawText());
        return OAuthTokenResponse.Success(payload);
    }

    private static string ComputeHmacSHA256(string key, string data)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var hash = HMACSHA256.HashData(keyBytes, dataBytes);

        return Convert.ToHexStringLower(hash);
    }

    private static string BuildSignatureString(SortedList<string, string> xbiliHeaders, string key)
    {
        var builder = new StringBuilder(256); // 256 is an estimated size for the plain text

        foreach ((var name, var value) in xbiliHeaders)
        {
            builder.Append(name)
                   .Append(':')
                   .Append(value)
                   .Append('\n');
        }

        var data = builder.ToString(0, builder.Length - 1); // Ignore the last '\n'
        return ComputeHmacSHA256(key, data);
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        request.Headers.Add("access-token", tokens.AccessToken);

        var xbiliHeaders = BuildXBiliHeaders();

        foreach ((var name, var value) in xbiliHeaders)
        {
            request.Headers.Add(name, value);
        }

        var signature = BuildSignatureString(xbiliHeaders, Options.ClientSecret);
        request.Headers.Add("Authorization", signature);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        var mainElement = payload.RootElement;

        if (!ValidateReturnCode(mainElement, out var code))
        {
            throw new AuthenticationFailureException($"An error (ErrorCode:{code}) occurred while retrieving user information.");
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, mainElement.GetProperty("data"));
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    private SortedList<string, string> BuildXBiliHeaders() => new(6, StringComparer.OrdinalIgnoreCase)
    {
        { "x-bili-accesskeyid", Options.ClientId },
        { "x-bili-content-md5", "d41d8cd98f00b204e9800998ecf8427e" }, // It's a GET request so there's no content, so we send the MD5 hash of an empty string
        { "x-bili-signature-method", "HMAC-SHA256" },
        { "x-bili-signature-nonce", GenerateNonce() },
        { "x-bili-signature-version", "2.0" },
        { "x-bili-timestamp", TimeProvider.GetUtcNow().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture) }
    };

    private static string GenerateNonce()
    {
        Span<byte> bytes = stackalloc byte[256 / 8];
        RandomNumberGenerator.Fill(bytes);
        return Base64Url.EncodeToString(bytes);
    }

    /// <summary>
    /// Check the code sent back by server for potential server errors.
    /// </summary>
    /// <param name="element">Main part of json document from response</param>
    /// <param name="code">Returned error_code from server</param>
    /// <remarks>See https://open.bilibili.com/doc/4/8673959e-f7bb-56e6-6e68-d225f971b81b#h1-u63A5u53E3u7B7Eu540Du5B9Eu73B0u6807u51C6u548Cu72B6u6001u7801 for details.</remarks>
    /// <returns>True if succeed, otherwise false.</returns>
    private static bool ValidateReturnCode(JsonElement element, out int code)
    {
        if (!element.TryGetProperty("code", out JsonElement errorCodeElement))
        {
            code = 0;
            return true;
        }

        code = errorCodeElement.GetInt32();
        return code == 0;
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

        [LoggerMessage(2, LogLevel.Warning, "An error occurred while retrieving the email address associated with the logged in user: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void EmailAddressError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
