/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Xero;

/// <summary>
/// Defines a handler for authentication using Xero.
/// </summary>
public partial class XeroAuthenticationHandler : OAuthHandler<XeroAuthenticationOptions>
{
    public XeroAuthenticationHandler(
        [NotNull] IOptionsMonitor<XeroAuthenticationOptions> options,
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
        await ProcessIdTokenAsync(tokens, properties, identity);

        var principal = new ClaimsPrincipal(identity);

        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, JsonDocument.Parse("{}").RootElement);
        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        var tokenRequestParameters = new Dictionary<string, string>
        {
            ["redirect_uri"] = context.RedirectUri,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code"
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
        request.Headers.Authorization = CreateAuthorizationHeader();

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

    private AuthenticationHeaderValue CreateAuthorizationHeader()
    {
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            string.Concat(
                EscapeDataString(Options.ClientId),
                ":",
                EscapeDataString(Options.ClientSecret))));

        return new AuthenticationHeaderValue("Basic", credentials);
    }

    private static string EscapeDataString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return Uri.EscapeDataString(value).Replace("%20", "+", StringComparison.Ordinal);
    }

    private async Task ProcessIdTokenAsync(
        [NotNull] OAuthTokenResponse tokens,
        [NotNull] AuthenticationProperties properties,
        [NotNull] ClaimsIdentity identity)
    {
        var idToken = tokens.Response!.RootElement.GetString("id_token");

        if (Options.SaveTokens)
        {
            // Save id_token as well.
            SaveIdToken(properties, idToken);
        }

        var tokenValidationResult = await ValidateAsync(idToken, Options.TokenValidationParameters.Clone());

        foreach (var claim in tokenValidationResult.ClaimsIdentity.Claims)
        {
            if (claim.Type == XeroAuthenticationConstants.ClaimNames.UserId)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                continue;
            }

            if (claim.Type == XeroAuthenticationConstants.ClaimNames.Email)
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, claim.Value));
                continue;
            }

            identity.AddClaim(new Claim(claim.Type, claim.Value));
        }
    }

    /// <summary>
    /// Store id_token in <paramref name="properties"/> token collection.
    /// </summary>
    /// <param name="properties">Authentication properties.</param>
    /// <param name="idToken">The id_token JWT.</param>
    private static void SaveIdToken(
        [NotNull] AuthenticationProperties properties,
        [NotNull] string? idToken)
    {
        if (!string.IsNullOrWhiteSpace(idToken))
        {
            // Get the currently available tokens
            var tokens = properties.GetTokens().ToList();

            // Add the extra token
            tokens.Add(new AuthenticationToken() { Name = "id_token", Value = idToken });

            // Overwrite store with original tokens with the new additional token
            properties.StoreTokens(tokens);
        }
    }

    private async Task<TokenValidationResult> ValidateAsync(
        [NotNull] string? idToken,
        [NotNull] TokenValidationParameters validationParameters)
    {
        if (Options.SecurityTokenHandler == null)
        {
            throw new InvalidOperationException("The options SecurityTokenHandler is null.");
        }

        if (!Options.SecurityTokenHandler.CanValidateToken)
        {
            throw new NotSupportedException($"The configured {nameof(JsonWebTokenHandler)} cannot validate tokens.");
        }

        if (Options.ConfigurationManager == null)
        {
            throw new InvalidOperationException($"An OpenID Connect configuration manager has not been set on the {nameof(XeroAuthenticationOptions)} instance.");
        }

        var openIdConnectConfiguration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
        validationParameters.IssuerSigningKeys = openIdConnectConfiguration.JsonWebKeySet.Keys;

        try
        {
            var result = await Options.SecurityTokenHandler.ValidateTokenAsync(idToken, validationParameters);
            if (result.Exception != null || !result.IsValid)
            {
                throw new SecurityTokenValidationException("Xero ID token validation failed.", result.Exception);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenValidationException("Xero ID token validation failed.", ex);
        }
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

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
