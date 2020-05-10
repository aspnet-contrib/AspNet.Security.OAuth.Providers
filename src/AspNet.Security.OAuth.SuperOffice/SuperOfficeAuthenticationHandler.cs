/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Defines a handler for authentication using SuperOffice.
    /// </summary>
    public class SuperOfficeAuthenticationHandler : OAuthHandler<SuperOfficeAuthenticationOptions>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public SuperOfficeAuthenticationHandler(
            [NotNull] IOptionsMonitor<SuperOfficeAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock,
            [NotNull] JwtSecurityTokenHandler tokenHandler)
            : base(options, logger, encoder, clock)
        {
            _tokenHandler = tokenHandler;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            // the access token is a reference token prefixed with the tenant context identifier
            string contextId = tokens.AccessToken[3..tokens.AccessToken.IndexOf('.', StringComparison.OrdinalIgnoreCase)];

            // add contextId to the Options.UserInformationEndpoint (https://sod.superoffice.com/{0}/api/v1/user/currentPrincipal)
            string userInfoEndpoint = string.Format(CultureInfo.InvariantCulture, Options.UserInformationEndpoint, contextId);

            string idToken = tokens.Response.RootElement.GetString("id_token");

            ClaimsPrincipal? tempClaimsPrincipal = null;

            if (Options.ValidateTokens)
            {
                tempClaimsPrincipal = await ValidateAsync(idToken);
            }

            if (Options.SaveTokens)
            {
                // Save id_token as well.
                SaveIdToken(properties, idToken);
            }

            if (Options.IncludeIdTokenAsClaims && tempClaimsPrincipal != null)
            {
                foreach (var claim in tempClaimsPrincipal.Claims)
                {
                    // may be possible same claim names from UserInformationEndpoint and IdToken.
                    if (!identity.HasClaim(c => c.Type == claim.Type))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }

            // Get the SuperOffice user principal
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
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException($"An error occurred when retrieving SuperOffice user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding SuperOffice API is enabled.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);

            if (Options.IncludeFunctionalRightsAsClaims
                && !Options.ClaimActions.Any(c => c.ClaimType == SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights))
            {
                Options.ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights, SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights);
            }

            context.RunClaimActions();
            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        private async Task<ClaimsPrincipal> ValidateAsync(string idToken)
        {
            if (!_tokenHandler.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JwtSecurityTokenHandler)} cannot validate tokens.");
            }

            if (Options.ConfigurationManager == null)
            {
                throw new InvalidOperationException($"The ConfigurationManager is null.");
            }

            var openIdConnectConfiguration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
            Options.TokenValidationParameters.IssuerSigningKeys = openIdConnectConfiguration.JsonWebKeySet.Keys;

            try
            {
                return _tokenHandler.ValidateToken(idToken, Options.TokenValidationParameters, out var _);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenValidationException(
                    "SuperOffice ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}.", ex);
            }
        }

        /// <summary>
        /// Store id_token in <paramref name="properties"/> token collection.
        /// </summary>
        /// <param name="properties">Authentication properties.</param>
        /// <param name="idToken">The id_token JWT.</param>
        private static void SaveIdToken(
            [NotNull] AuthenticationProperties properties,
            [NotNull] string idToken)
        {
            if (!string.IsNullOrWhiteSpace(idToken))
            {
                // save existing tokens, which are removed in StoreTokens method.
                var tokens = properties.GetTokens().ToList();
                tokens.Add(new AuthenticationToken() { Name = "id_token", Value = idToken });
                properties.StoreTokens(tokens);
            }
        }
    }
}
