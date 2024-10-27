/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Base64UrlEncoder = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder;

namespace AspNet.Security.OAuth.VkId;

public sealed class VkIdAuthenticationHandler : OAuthHandler<VkIdAuthenticationOptions>
{
    public VkIdAuthenticationHandler(
        IOptionsMonitor<VkIdAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
    {
        var parameter = Options.Scope;
        var scopes = FormatScope(parameter);

        var data = RandomNumberGenerator.GetBytes(32);
        var codeVerifierKey = Base64UrlEncoder.Encode(data);
        properties.Items.Add(OAuthConstants.CodeVerifierKey, codeVerifierKey);

        var query = new Dictionary<string, string?>
        {
            ["response_type"] = "code",
            ["client_id"] = Options.ClientId,
            ["scope"] = scopes,
            ["redirect_uri"] = redirectUri,
            ["state"] = Options.StateDataFormat.Protect(properties),
            ["code_challenge"] = WebEncoders.Base64UrlEncode(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifierKey))),
            ["code_challenge_method"] = OAuthConstants.CodeChallengeMethodS256
        };
        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, query);
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        var properties = Options.StateDataFormat.Unprotect(Request.Query["state"]);
        if (properties is null)
        {
            return HandleRequestResult.Fail("The oauth state was missing or invalid.");
        }

        if (ValidateCorrelationId(properties) is false)
        {
            return HandleRequestResult.Fail("Correlation failed.");
        }

        var code = Request.Query["code"];
        if (StringValues.IsNullOrEmpty(code))
        {
            return HandleRequestResult.Fail("Code was not found.");
        }

        var deviceId = Request.Query["device_id"];
        if (StringValues.IsNullOrEmpty(deviceId))
        {
            return HandleRequestResult.Fail("Device ID was not found.");
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

        if (string.IsNullOrEmpty(tokens.RefreshToken))
        {
            return HandleRequestResult.Fail("Failed to retrieve refresh token.", properties);
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
                new()
                {
                    Name = "refresh_token",
                    Value = tokens.RefreshToken,
                },
            };

            if (tokens.Response!.RootElement.GetString("id_token") is { } idToken)
            {
                tokensToStore.Add(new AuthenticationToken
                {
                    Name = "id_token",
                    Value = idToken
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

            properties.StoreTokens(tokensToStore);
        }

        var identity = new ClaimsIdentity(ClaimsIssuer);
        var ticket = await CreateTicketAsync(identity, properties, tokens);
        return HandleRequestResult.Success(ticket);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
    {
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
        var query = new Dictionary<string, string>()
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
        request.Version = Backchannel.DefaultRequestVersion;

        var response = await Backchannel.SendAsync(request, Context.RequestAborted);
        var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        if (!payload.RootElement.TryGetProperty("state", out var state) ||
            Options.StateDataFormat.Unprotect(state.GetString()) is null)
        {
            return OAuthTokenResponse.Failed(new Exception("The oauth state was missing or invalid."));
        }

        if (!payload.RootElement.TryGetProperty("error", out var errorElement))
        {
            return OAuthTokenResponse.Success(payload);
        }

        var errorCode = errorElement.GetString()!;
        var errorDescription = errorElement.GetProperty("error_description").GetString()!;
        return OAuthTokenResponse.Failed(new Exception($"{errorCode}: {errorDescription}"));
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
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
}
