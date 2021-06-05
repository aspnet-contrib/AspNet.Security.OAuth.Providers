/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Defines a handler for authentication using Apple.
    /// </summary>
    public class AppleAuthenticationHandler : OAuthHandler<AppleAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        public AppleAuthenticationHandler(
            [NotNull] IOptionsMonitor<AppleAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// The handler calls methods on the events which give the application control at certain points where processing is occurring.
        /// If it is not provided a default instance is supplied which does nothing when the methods are called.
        /// </summary>
        protected new AppleAuthenticationEvents Events
        {
            get { return (AppleAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        /// <inheritdoc />
        protected override string BuildChallengeUrl(
            [NotNull] AuthenticationProperties properties,
            [NotNull] string redirectUri)
        {
            string challengeUrl = base.BuildChallengeUrl(properties, redirectUri);

            // Apple requires the response mode to be form_post when the email or name scopes are requested
            return QueryHelpers.AddQueryString(challengeUrl, "response_mode", "form_post");
        }

        /// <inheritdoc />
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new AppleAuthenticationEvents());

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            string? idToken = tokens.Response!.RootElement.GetString("id_token");

            Logger.LogInformation("Creating ticket for Sign in with Apple.");

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace("Access Token: {AccessToken}", tokens.AccessToken);
                Logger.LogTrace("Refresh Token: {RefreshToken}", tokens.RefreshToken);
                Logger.LogTrace("Token Type: {TokenType}", tokens.TokenType);
                Logger.LogTrace("Expires In: {ExpiresIn}", tokens.ExpiresIn);
                Logger.LogTrace("Response: {TokenResponse}", tokens.Response.RootElement);
                Logger.LogTrace("ID Token: {IdToken}", idToken);
            }

            if (string.IsNullOrWhiteSpace(idToken))
            {
                throw new InvalidOperationException("No Apple ID token was returned in the OAuth token response.");
            }

            if (Options.ValidateTokens)
            {
                var validateIdContext = new AppleValidateIdTokenContext(Context, Scheme, Options, idToken);
                await Options.Events.ValidateIdToken(validateIdContext);
            }

            var tokenClaims = ExtractClaimsFromToken(idToken);

            foreach (var claim in tokenClaims)
            {
                identity.AddClaim(claim);
            }

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, tokens.Response.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        /// <inheritdoc />
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            if (Options.GenerateClientSecret)
            {
                var secretGenerationContext = new AppleGenerateClientSecretContext(Context, Scheme, Options);
                await Events.GenerateClientSecret(secretGenerationContext);
            }

            return await base.ExchangeCodeAsync(context);
        }

        /// <summary>
        /// Extracts the claims from the token received from the token endpoint.
        /// </summary>
        /// <param name="token">The token to extract the claims from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{Claim}"/> containing the claims extracted from the token.
        /// </returns>
        protected virtual IEnumerable<Claim> ExtractClaimsFromToken([NotNull] string token)
        {
            try
            {
                var securityToken = Options.SecurityTokenHandler.ReadJsonWebToken(token);

                return new List<Claim>(securityToken.Claims)
                {
                    new Claim(ClaimTypes.NameIdentifier, securityToken.Subject, ClaimValueTypes.String, ClaimsIssuer),
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse JWT for claims from Apple ID token.", ex);
            }
        }

        /// <summary>
        /// Extracts the claims from the user received from the authorization endpoint.
        /// </summary>
        /// <param name="user">The user object to extract the claims from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{Claim}"/> containing the claims extracted from the user information.
        /// </returns>
        protected virtual IEnumerable<Claim> ExtractClaimsFromUser([NotNull] JsonElement user)
        {
            var claims = new List<Claim>();

            if (user.TryGetProperty("name", out var name))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, name.GetString("firstName") ?? string.Empty, ClaimValueTypes.String, ClaimsIssuer));
                claims.Add(new Claim(ClaimTypes.Surname, name.GetString("lastName") ?? string.Empty, ClaimValueTypes.String, ClaimsIssuer));
            }

            if (user.TryGetProperty("email", out var email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email.GetString() ?? string.Empty, ClaimValueTypes.String, ClaimsIssuer));
            }

            return claims;
        }

        /// <inheritdoc />
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            IEnumerable<KeyValuePair<string, StringValues>> source;

            // If form_post was used, then read the parameters from the form rather than the query string
            if (string.Equals(Request.Method, HttpMethod.Post.Method, StringComparison.OrdinalIgnoreCase))
            {
                source = Request.Form;
            }
            else
            {
                source = Request.Query;
            }

            var parameters = new Dictionary<string, StringValues>(source);

            return await HandleRemoteAuthenticateAsync(parameters);
        }

        private async Task<HandleRequestResult> HandleRemoteAuthenticateAsync(
            [NotNull] Dictionary<string, StringValues> parameters)
        {
            // Adapted from https://github.com/aspnet/AspNetCore/blob/66de493473521e173fa15ca557f5f97601cedb23/src/Security/Authentication/OAuth/src/OAuthHandler.cs#L48-L175
            if (!parameters.TryGetValue("state", out var state))
            {
                state = default;
            }

            var properties = Options.StateDataFormat.Unprotect(state);

            if (properties == null)
            {
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");
            }

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            if (!parameters.TryGetValue("error", out var error))
            {
                error = default;
            }

            if (!StringValues.IsNullOrEmpty(error))
            {
                // Note: access_denied errors are special protocol errors indicating the user didn't
                // approve the authorization demand requested by the remote authorization server.
                // Since it's a frequent scenario (that is not caused by incorrect configuration),
                // denied errors are handled differently using HandleAccessDeniedErrorAsync().
                // Visit https://tools.ietf.org/html/rfc6749#section-4.1.2.1 for more information.
                if (!parameters.TryGetValue("error_description", out var errorDescription))
                {
                    errorDescription = default;
                }

                if (!parameters.TryGetValue("error_uri", out var errorUri))
                {
                    errorUri = default;
                }

                // See https://developer.apple.com/documentation/sign_in_with_apple/sign_in_with_apple_js/incorporating_sign_in_with_apple_into_other_platforms.
                if (StringValues.Equals(error, "access_denied") || StringValues.Equals(error, "user_cancelled_authorize"))
                {
                    var result = await HandleAccessDeniedErrorAsync(properties);

                    if (!result.None)
                    {
                        return result;
                    }

                    var deniedEx = new Exception("Access was denied by the resource owner or by the remote server.");

                    deniedEx.Data["error"] = error.ToString();
                    deniedEx.Data["error_description"] = errorDescription.ToString();
                    deniedEx.Data["error_uri"] = errorUri.ToString();

                    return HandleRequestResult.Fail(deniedEx, properties);
                }

                var failureMessage = new StringBuilder().Append(error);

                if (!StringValues.IsNullOrEmpty(errorDescription))
                {
                    failureMessage.Append(";Description=").Append(errorDescription);
                }

                if (!StringValues.IsNullOrEmpty(errorUri))
                {
                    failureMessage.Append(";Uri=").Append(errorUri);
                }

                var ex = new Exception(failureMessage.ToString());

                ex.Data["error"] = error.ToString();
                ex.Data["error_description"] = errorDescription.ToString();
                ex.Data["error_uri"] = errorUri.ToString();

                return HandleRequestResult.Fail(ex, properties);
            }

            if (!parameters.TryGetValue("code", out var code))
            {
                code = default;
            }

            if (StringValues.IsNullOrEmpty(code))
            {
                return HandleRequestResult.Fail("Code was not found.", properties);
            }

            var codeExchangeContext = new OAuthCodeExchangeContext(properties, code, BuildRedirectUri(Options.CallbackPath));

            using var tokens = await ExchangeCodeAsync(codeExchangeContext);

            if (tokens.Error != null)
            {
                return HandleRequestResult.Fail(tokens.Error, properties);
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
            }

            var identity = new ClaimsIdentity(ClaimsIssuer);

            if (Options.SaveTokens)
            {
                var authTokens = new List<AuthenticationToken>()
                {
                    new AuthenticationToken() { Name = "access_token", Value = tokens.AccessToken },
                };

                if (!string.IsNullOrEmpty(tokens.RefreshToken))
                {
                    authTokens.Add(new AuthenticationToken() { Name = "refresh_token", Value = tokens.RefreshToken });
                }

                if (!string.IsNullOrEmpty(tokens.TokenType))
                {
                    authTokens.Add(new AuthenticationToken() { Name = "token_type", Value = tokens.TokenType });
                }

                if (!string.IsNullOrEmpty(tokens.ExpiresIn))
                {
                    if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                    {
                        // https://www.w3.org/TR/xmlschema-2/#dateTime
                        // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
                        var expiresAt = Clock.UtcNow + TimeSpan.FromSeconds(value);

                        authTokens.Add(new AuthenticationToken()
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture),
                        });
                    }
                }

                string? idToken = tokens.Response?.RootElement.GetString("id_token");

                if (!string.IsNullOrEmpty(idToken))
                {
                    authTokens.Add(new AuthenticationToken() { Name = "id_token", Value = idToken });
                }

                properties.StoreTokens(authTokens);
            }

            if (parameters.TryGetValue("user", out var userJson))
            {
                using var user = JsonDocument.Parse(userJson);
                var userClaims = ExtractClaimsFromUser(user.RootElement);

                foreach (var claim in userClaims)
                {
                    identity.AddClaim(claim);
                }
            }

            var ticket = await CreateTicketAsync(identity, properties, tokens);

            if (ticket != null)
            {
                return HandleRequestResult.Success(ticket);
            }
            else
            {
                return HandleRequestResult.Fail("Failed to retrieve user information from remote server.", properties);
            }
        }
    }
}
