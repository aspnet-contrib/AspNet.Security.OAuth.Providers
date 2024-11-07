/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
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
using Base64UrlEncoder = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder;

namespace AspNet.Security.OAuth.VkId;

public sealed partial class VkIdAuthenticationHandler : OAuthHandler<VkIdAuthenticationOptions>
{
    public VkIdAuthenticationHandler(
        [NotNull] IOptionsMonitor<VkIdAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override string BuildChallengeUrl(
        [NotNull] AuthenticationProperties properties,
        [NotNull] string redirectUri)
    {
        // It's mandatory to use PKCE
        var data = RandomNumberGenerator.GetBytes(32);
        var codeVerifierKey = Base64UrlEncoder.Encode(data);
        properties.Items.Add(OAuthConstants.CodeVerifierKey, codeVerifierKey);

        var query = new Dictionary<string, string?>
        {
            ["response_type"] = "code",
            ["client_id"] = Options.ClientId,
            ["scope"] = FormatScope(Options.Scope),
            ["redirect_uri"] = redirectUri,
            ["state"] = Options.StateDataFormat.Protect(properties),
            ["code_challenge"] = WebEncoders.Base64UrlEncode(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifierKey))),
            ["code_challenge_method"] = OAuthConstants.CodeChallengeMethodS256
        };
        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, query);
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        var query = Request.Query;
        var properties = Options.StateDataFormat.Unprotect(query["state"]);
        if (properties is null)
        {
            return HandleRequestResult.Fail("The oauth state was missing or invalid.");
        }

        // OAuth2 10.12 CSRF
        if (!ValidateCorrelationId(properties))
        {
            return HandleRequestResult.Fail("Correlation failed.", properties);
        }

        // According to docs query cannot contain errors but VK documentation tends to lie so debug log here
        Log.CodeResponse(Logger, query);

        var code = Request.Query["code"];
        if (StringValues.IsNullOrEmpty(code))
        {
            return HandleRequestResult.Fail("Code was not found.", properties);
        }

        var deviceId = Request.Query["device_id"];
        if (StringValues.IsNullOrEmpty(deviceId))
        {
            return HandleRequestResult.Fail("Device ID was not found.", properties);
        }

        properties.Items.Add(VkIdAuthenticationConstants.AuthenticationProperties.DeviceId, deviceId);
        var codeExchangeContext = new OAuthCodeExchangeContext(
            properties,
            code!,
            BuildRedirectUri(Options.CallbackPath));

        using var tokens = await ExchangeCodeAsync(codeExchangeContext);
        if (tokens.Error is not null)
        {
            return HandleRequestResult.Fail(tokens.Error, properties);
        }

        if (string.IsNullOrEmpty(tokens.AccessToken))
        {
            return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
        }

        if (Options.SaveTokens)
        {
            var tokensToStore = new List<AuthenticationToken>
            {
                new()
                {
                    Name = "access_token",
                    Value = tokens.AccessToken,
                },
            };

            if (!string.IsNullOrEmpty(tokens.RefreshToken))
            {
                tokensToStore.Add(new AuthenticationToken
                {
                    Name = "refresh_token",
                    Value = tokens.RefreshToken,
                });
            }

            if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out var expiresIn))
            {
                var expiresAt = TimeProvider
                    .GetUtcNow()
                    .AddSeconds(expiresIn);

                tokensToStore.Add(new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                });
            }

            if (!string.IsNullOrEmpty(tokens.TokenType))
            {
                tokensToStore.Add(new AuthenticationToken
                {
                    Name = "token_type",
                    Value = tokens.TokenType
                });
            }

            properties.StoreTokens(tokensToStore);
        }

        var identity = new ClaimsIdentity(ClaimsIssuer);
        var ticket = await CreateTicketAsync(identity, properties, tokens);
        return HandleRequestResult.Success(ticket);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // Both device_id and code_verifier are required to get access token
        if (!context.Properties.Items.TryGetValue(VkIdAuthenticationConstants.AuthenticationProperties.DeviceId, out var deviceId) ||
            string.IsNullOrEmpty(deviceId))
        {
            return OAuthTokenResponse.Failed(new Exception("Device ID was not found."));
        }

        if (!context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier) ||
            string.IsNullOrEmpty(codeVerifier))
        {
            return OAuthTokenResponse.Failed(new Exception("Code verifier key was not found."));
        }

        context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        var query = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = context.Code,
            ["code_verifier"] = codeVerifier,
            ["client_id"] = Options.ClientId,
            ["device_id"] = deviceId,
            ["redirect_uri"] = context.RedirectUri,
            ["state"] = Options.StateDataFormat.Protect(context.Properties),
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new FormUrlEncodedContent(query);

        var response = await Backchannel.SendAsync(request, Context.RequestAborted);

        // According to docs even error response should be 200
        if (response.StatusCode is not HttpStatusCode.OK)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("Invalid remote server response during code exchange."));
        }

        var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        // ReSharper disable once InvertIf
        if (payload.RootElement.TryGetProperty("error", out var errorElement))
        {
            var errorCode = errorElement.GetString()!;
            var errorDescription = errorElement.GetProperty("error_description").GetString()!;
            return OAuthTokenResponse.Failed(new Exception($"{errorCode}: {errorDescription}"));
        }

        return OAuthTokenResponse.Success(payload);
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        var query = new Dictionary<string, string>
        {
            ["access_token"] = tokens.AccessToken!,
            ["client_id"] = Options.ClientId
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, Options.UserInformationEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new FormUrlEncodedContent(query);
        request.Version = Backchannel.DefaultRequestVersion;

        var response = await Backchannel.SendAsync(request, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        var content = await response.Content.ReadAsStringAsync(Context.RequestAborted);
        var body = JsonDocument.Parse(content);

        if (body.RootElement.TryGetProperty("error", out var errorElement))
        {
            var errorCode = errorElement.GetString();
            var errorDescription = body.RootElement
                .GetProperty("error_description")
                .GetString();

            throw new Exception($"{errorCode}: {errorDescription}");
        }

        if (!body.RootElement.TryGetProperty("user", out var payload))
        {
            Log.FailedToRetrieveUserInformation(Logger, response, content);
            throw new Exception("Failed to retrieve user information.");
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(
            principal,
            properties,
            Context,
            Scheme,
            Options,
            Backchannel,
            tokens,
            payload);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    private static partial class Log
    {
        internal static void CodeResponse(ILogger logger, IQueryCollection? query)
        {
            CodeResponse(logger, query?.ToString() ?? string.Empty);
        }

        internal static async Task ExchangeCodeErrorAsync(
            ILogger logger,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            ExchangeCodeErrorAsync(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserProfileError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static void FailedToRetrieveUserInformation(
            ILogger logger,
            HttpResponseMessage response,
            string content)
        {
            FailedToRetrieveUserInformation(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                content);
        }

        [LoggerMessage(1, LogLevel.Debug, "Authorization endpoint callback query: {Query}.")]
        internal static partial void CodeResponse(ILogger logger, string query);

        [LoggerMessage(2, LogLevel.Error, "Invalid server response while retrieving an OAuth token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeErrorAsync(
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

        [LoggerMessage(4, LogLevel.Error, "Failed to retrieve user information: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void FailedToRetrieveUserInformation(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
