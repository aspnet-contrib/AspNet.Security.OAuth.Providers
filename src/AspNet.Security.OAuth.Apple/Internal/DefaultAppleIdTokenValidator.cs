/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleIdTokenValidator : AppleIdTokenValidator
    {
        private readonly ILogger _logger;

        public DefaultAppleIdTokenValidator(
            [NotNull] ILogger<DefaultAppleIdTokenValidator> logger)
        {
            _logger = logger;
        }

        public override async Task ValidateAsync([NotNull] AppleValidateIdTokenContext context)
        {
            if (context.Options.SecurityTokenHandler is null)
            {
                throw new InvalidOperationException("The options SecurityTokenHandler is null.");
            }

            if (!context.Options.SecurityTokenHandler.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JsonWebTokenHandler)} cannot validate tokens.");
            }

            if (context.Options.ConfigurationManager is null)
            {
                throw new InvalidOperationException($"An OpenID Connect configuration manager has not been set on the {nameof(AppleAuthenticationOptions)} instance.");
            }

            if (context.Options.TokenValidationParameters is null)
            {
                throw new InvalidOperationException($"Token validation parameters have not been set on the {nameof(AppleAuthenticationOptions)} instance.");
            }

            OpenIdConnectConfiguration configuration = await context.Options.ConfigurationManager.GetConfigurationAsync(context.HttpContext.RequestAborted);

            context.Options.TokenValidationParameters.IssuerSigningKeys = configuration.JsonWebKeySet.Keys;

            try
            {
                var result = context.Options.SecurityTokenHandler.ValidateToken(context.IdToken, context.Options.TokenValidationParameters);

                if (result.Exception != null || !result.IsValid)
                {
                    throw new SecurityTokenValidationException("Apple ID token validation failed.", result.Exception);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Apple ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}.",
                    context.Options.TokenValidationParameters.ValidIssuer,
                    context.Options.TokenValidationParameters.ValidAudience);

                _logger.LogTrace(
                    ex,
                    "Apple ID token {IdToken} could not be validated.",
                    context.IdToken);

                throw;
            }
        }
    }
}
