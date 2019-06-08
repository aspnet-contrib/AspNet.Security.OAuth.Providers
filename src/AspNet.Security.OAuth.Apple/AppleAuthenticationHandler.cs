/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
    }
}
