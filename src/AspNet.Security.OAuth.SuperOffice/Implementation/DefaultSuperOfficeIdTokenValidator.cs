/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice.Implementation
{
    internal sealed class DefaultSuperOfficeIdTokenValidator : SuperOfficeIdTokenValidator
    {
        private readonly ILogger _logger;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public DefaultSuperOfficeIdTokenValidator(
            [NotNull] JwtSecurityTokenHandler tokenHandler,
            [NotNull] ILogger<DefaultSuperOfficeIdTokenValidator> logger)
        {
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        public override async Task ValidateAsync([NotNull] SuperOfficeValidateIdTokenContext context)
        {
            if (!_tokenHandler.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JwtSecurityTokenHandler)} cannot validate tokens.");
            }

            if (context.Options.ConfigurationManager != null)
            {
                var openIdConnectConfiguration = await context.Options.ConfigurationManager.GetConfigurationAsync(CancellationToken.None);
                context.Options.TokenValidationParameters.IssuerSigningKeys = openIdConnectConfiguration.JsonWebKeySet.Keys;

                try
                {
                    _tokenHandler.ValidateToken(context.IdToken, context.Options.TokenValidationParameters, out var _);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "SuperOffice ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}.",
                        context.Options.TokenValidationParameters.ValidIssuer,
                        context.Options.TokenValidationParameters.ValidAudience);

                    _logger.LogTrace(
                        ex,
                        "SuperOffice ID token {IdToken} could not be validated.",
                        context.IdToken);

                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException($"The ConfigurationManager is null.");
            }
        }
    }
}
