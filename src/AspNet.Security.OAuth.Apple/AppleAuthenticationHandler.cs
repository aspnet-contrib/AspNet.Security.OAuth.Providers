/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
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
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        /// <param name="tokenHandler">The JWT security token handler to use.</param>
        public AppleAuthenticationHandler(
            [NotNull] IOptionsMonitor<AppleAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock,
            [NotNull] JwtSecurityTokenHandler tokenHandler)
            : base(options, logger, encoder, clock)
        {
            _tokenHandler = tokenHandler;
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
            string idToken = tokens.Response.Value<string>("id_token");

            // TODO These can probably be removed once Sign In with Apple is finalized
            Logger.LogInformation("Creating ticket for Sign In with Apple.");
            Logger.LogTrace("Access Token: {AccessToken}", tokens.AccessToken);
            Logger.LogTrace("Refresh Token: {RefreshToken}", tokens.RefreshToken);
            Logger.LogTrace("Token Type: {TokenType}", tokens.TokenType);
            Logger.LogTrace("Expires In: {ExpiresIn}", tokens.ExpiresIn);
            Logger.LogTrace("Response: {TokenResponse}", tokens.Response);
            Logger.LogTrace("ID Token: {IdToken}", idToken);

            if (string.IsNullOrWhiteSpace(idToken))
            {
                throw new InvalidOperationException("No Apple ID token was returned in the OAuth token response.");
            }

            if (Options.ValidateTokens)
            {
                var validateIdContext = new AppleValidateIdTokenContext(Context, Scheme, Options, idToken);
                await Events.ValidateIdToken(validateIdContext);
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, GetNameIdentifier(idToken)));

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens);
            context.RunClaimActions(tokens.Response);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        /// <inheritdoc />
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            if (Options.GenerateClientSecret)
            {
                var context = new AppleGenerateClientSecretContext(Context, Scheme, Options);
                await Events.GenerateClientSecret(context);
            }

            return await base.ExchangeCodeAsync(code, redirectUri);
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

            var parameters = new Dictionary<string, StringValues>();

            foreach (var param in source)
            {
                parameters.Add(param.Key, param.Value);
            }

            return await HandleRemoteAuthenticateAsync(parameters);
        }

        private string GetNameIdentifier(string token)
        {
            try
            {
                var userToken = _tokenHandler.ReadJwtToken(token);
                return userToken.Subject;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse JWT from Apple ID token.", ex);
            }
        }

        private async Task<HandleRequestResult> HandleRemoteAuthenticateAsync(
            [NotNull] Dictionary<string, StringValues> parameters)
        {
            // Adapted from https://github.com/aspnet/AspNetCore/blob/e7f262e33108e92fc8805b925cc04b07d254118b/src/Security/Authentication/OAuth/src/OAuthHandler.cs#L45-L146
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
                return HandleRequestResult.Fail("Correlation failed.");
            }

            if (!parameters.TryGetValue("error", out var error))
            {
                error = default;
            }

            if (!StringValues.IsNullOrEmpty(error))
            {
                var failureMessage = new StringBuilder().Append(error);

                if (!parameters.TryGetValue("error_description", out var errorDescription))
                {
                    errorDescription = default;
                }

                if (!StringValues.IsNullOrEmpty(errorDescription))
                {
                    failureMessage.Append(";Description=").Append(errorDescription);
                }

                if (!parameters.TryGetValue("error_uri", out var errorUri))
                {
                    errorUri = default;
                }

                if (!StringValues.IsNullOrEmpty(errorUri))
                {
                    failureMessage.Append(";Uri=").Append(errorUri);
                }

                return HandleRequestResult.Fail(failureMessage.ToString());
            }

            if (!parameters.TryGetValue("code", out var code))
            {
                code = default;
            }

            if (StringValues.IsNullOrEmpty(code))
            {
                return HandleRequestResult.Fail("Code was not found.");
            }

            var tokens = await ExchangeCodeAsync(code, BuildRedirectUri(Options.CallbackPath));

            if (tokens.Error != null)
            {
                return HandleRequestResult.Fail(tokens.Error);
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.");
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

                properties.StoreTokens(authTokens);
            }

            var ticket = await CreateTicketAsync(identity, properties, tokens);

            if (ticket != null)
            {
                return HandleRequestResult.Success(ticket);
            }
            else
            {
                return HandleRequestResult.Fail("Failed to retrieve user information from remote server.");
            }
        }
    }
}
