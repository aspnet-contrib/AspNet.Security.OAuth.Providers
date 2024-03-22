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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Shopify;

public partial class ShopifyAuthenticationHandler : OAuthHandler<ShopifyAuthenticationOptions>
{
    public ShopifyAuthenticationHandler(
        [NotNull] IOptionsMonitor<ShopifyAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <inheritdoc />
    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
#pragma warning disable CA1863
        var uri = string.Format(
            CultureInfo.InvariantCulture,
            ShopifyAuthenticationDefaults.UserInformationEndpointFormat,
            properties.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty]);
#pragma warning restore CA1863

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("X-Shopify-Access-Token", tokens.AccessToken);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the shop profile.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        // In Shopify, the customer can modify the scope given to the app. Apps should verify
        // that the customer is allowing the required scope.
        var actualScope = tokens.Response!.RootElement.GetString("scope") ?? string.Empty;
        var isPersistent = true;

        // If the request was for a "per-user" (i.e. no offline access)
        if (tokens.Response.RootElement.TryGetProperty("expires_in", out var expiresInProperty))
        {
            isPersistent = false;

            if (expiresInProperty.TryGetInt32(out var expiresIn))
            {
                var expires = TimeProvider.GetUtcNow().AddSeconds(expiresIn);
                identity.AddClaim(new Claim(ClaimTypes.Expiration, expires.ToString("O", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime));
            }

            actualScope = tokens.Response.RootElement.GetString("associated_user_scope") ?? string.Empty;

            var userData = tokens.Response.RootElement.GetString("associated_user") ?? string.Empty;
            identity.AddClaim(new Claim(ClaimTypes.UserData, userData));
        }

        identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isPersistent.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Boolean));
        identity.AddClaim(new Claim(ShopifyAuthenticationDefaults.ShopifyScopeClaimType, actualScope));

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <inheritdoc />
    protected override string FormatScope([NotNull] IEnumerable<string> scopes)
        => string.Join(',', scopes);

    /// <inheritdoc />
    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
    {
        if (!properties.Items.TryGetValue(ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty, out var shopName))
        {
            Log.ShopNameMissing(Logger);
            throw new AuthenticationFailureException("Shopify provider AuthenticationProperties must contain ShopNameAuthenticationProperty.");
        }

        var authorizationEndpoint = string.Format(CultureInfo.InvariantCulture, Options.AuthorizationEndpoint, shopName);

        // Get the permission scope, which can either be set in options or overridden in AuthenticationProperties.
        if (!properties.Items.TryGetValue(ShopifyAuthenticationDefaults.ShopScopeAuthenticationProperty, out var scope))
        {
            var scopeParameter = properties.GetParameter<ICollection<string>>(OAuthChallengeProperties.ScopeKey);
            scope = scopeParameter != null ? FormatScope(scopeParameter) : FormatScope();
        }

        var parameters = new Dictionary<string, string?>()
        {
            ["client_id"] = Options.ClientId,
            ["scope"] = scope,
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

        var challengeUrl = QueryHelpers.AddQueryString(authorizationEndpoint, parameters);

        // If we're requesting a per-user, online only, token, add the grant_options query param.
        if (properties.Items.TryGetValue(ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty, out var grantOptions) &&
            grantOptions == ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue)
        {
            challengeUrl = QueryHelpers.AddQueryString(challengeUrl, "grant_options[]", ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue);
        }

        return challengeUrl;
    }

    /// <inheritdoc />
    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        var tokenRequestParameters = new Dictionary<string, string>
        {
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret,
            ["code"] = context.Code,
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        string shopDns;

        try
        {
            var shopValue = Context.Request.Query["shop"];
            var stateValue = Context.Request.Query["state"];

            var shop = shopValue.ToString();

            // Shop name must end with myshopify.com
            if (!shop.EndsWith(".myshopify.com", StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthenticationFailureException("shop parameter is malformed. It should end with .myshopify.com");
            }

            // Strip out the "myshopify.com" suffix
            shopDns = shop.Split('.')[0];

            // Verify that the shop name encoded in "state" matches the shop name we used to
            // request the token. This probably isn't necessary, but it's an easy extra verification.
            var authenticationProperties = Options.StateDataFormat.Unprotect(stateValue);

            var shopNamePropertyValue = authenticationProperties?.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty];

            if (!string.Equals(shopNamePropertyValue, shopDns, StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthenticationFailureException("Received shop name does not match the shop name specified in the authentication request.");
            }
        }
        catch (Exception ex)
        {
            Log.TokenExchangeError(Logger, ex, ex.Message);
            return OAuthTokenResponse.Failed(ex);
        }

        var uri = string.Format(CultureInfo.InvariantCulture, Options.TokenEndpoint, shopDns);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        request.Content = new FormUrlEncodedContent(tokenRequestParameters);

        using var response = await Backchannel.SendAsync(request, Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
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

        [LoggerMessage(3, LogLevel.Error, "An error occurred while exchanging tokens: {ErrorMessage}")]
        internal static partial void TokenExchangeError(ILogger logger, Exception exception, string errorMessage);

        [LoggerMessage(4, LogLevel.Error, "Shopify provider AuthenticationProperties must contain ShopNameAuthenticationProperty.")]
        internal static partial void ShopNameMissing(ILogger logger);

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
