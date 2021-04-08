/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Defines a handler for authentication using SuperOffice.
    /// </summary>
    public class SuperOfficeAuthenticationHandler : OAuthHandler<SuperOfficeAuthenticationOptions>
    {
        public SuperOfficeAuthenticationHandler(
            [NotNull] IOptionsMonitor<SuperOfficeAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var contextId = await ProcessIdTokenAndGetContactIdentifierAsync(tokens, properties, identity);

            if (string.IsNullOrEmpty(contextId))
            {
                throw new InvalidOperationException("An error occurred trying to obtain the context identifier from the current user's identity claims.");
            }

            // Add contextId to the Options.UserInformationEndpoint (https://sod.superoffice.com/{0}/api/v1/user/currentPrincipal).
            var userInfoEndpoint = string.Format(CultureInfo.InvariantCulture, Options.UserInformationEndpoint, contextId);

            // Get the SuperOffice user principal.
            using var request = new HttpRequestMessage(HttpMethod.Get, userInfoEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException($"An error occurred when retrieving SuperOffice user information ({response.StatusCode}).");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        private async Task<string> ProcessIdTokenAndGetContactIdentifierAsync(
            [NotNull] OAuthTokenResponse tokens,
            [NotNull] AuthenticationProperties properties,
            [NotNull] ClaimsIdentity identity)
        {
            string? idToken = tokens.Response!.RootElement.GetString("id_token");

            if (Options.SaveTokens)
            {
                // Save id_token as well.
                SaveIdToken(properties, idToken);
            }

            var tokenValidationResult = await ValidateAsync(idToken, Options.TokenValidationParameters.Clone());

            var contextIdentifier = string.Empty;

            foreach (var claim in tokenValidationResult.ClaimsIdentity.Claims)
            {
                if (claim.Type == SuperOfficeAuthenticationConstants.ClaimNames.ContextIdentifier)
                {
                    contextIdentifier = claim.Value;
                }

                if (claim.Type == SuperOfficeAuthenticationConstants.ClaimNames.SubjectIdentifier)
                {
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                    continue;
                }

                if (Options.IncludeIdTokenAsClaims)
                {
                    // May be possible same claim names from UserInformationEndpoint and IdToken.
                    if (!identity.HasClaim(c => c.Type == claim.Type))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }

            return contextIdentifier;
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
                throw new InvalidOperationException($"An OpenID Connect configuration manager has not been set on the {nameof(SuperOfficeAuthenticationOptions)} instance.");
            }

            var openIdConnectConfiguration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
            validationParameters.IssuerSigningKeys = openIdConnectConfiguration.JsonWebKeySet.Keys;

            try
            {
                var result = Options.SecurityTokenHandler.ValidateToken(idToken, validationParameters);

                if (result.Exception != null || !result.IsValid)
                {
                    throw new SecurityTokenValidationException("SuperOffice ID token validation failed.", result.Exception);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenValidationException("SuperOffice ID token validation failed.", ex);
            }
        }
    }
}
