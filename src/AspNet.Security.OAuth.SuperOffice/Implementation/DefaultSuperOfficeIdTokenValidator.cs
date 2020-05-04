/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice.Implementation
{
    internal sealed class DefaultSuperOfficeIdTokenValidator : SuperOfficeIdTokenValidator
    {
        private readonly ILogger _logger;
        private readonly SuperOfficeAuthenticationConfigurationManager _configManager;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public DefaultSuperOfficeIdTokenValidator(
            SuperOfficeAuthenticationConfigurationManager configManager,
            JwtSecurityTokenHandler tokenHandler,
            ILogger<DefaultSuperOfficeIdTokenValidator> logger)
        {
            _configManager = configManager;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        public override async Task ValidateAsync(SuperOfficeValidateIdTokenContext context)
        {
            if (!_tokenHandler.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JwtSecurityTokenHandler)} cannot validate tokens.");
            }

            var keySet = await _configManager.GetPublicKeySetAsync(context);

            var parameters = new TokenValidationParameters()
            {
                ValidAudience = context.Options.ClientId,
                ValidIssuer = context.Options.ClaimsIssuer,
                IssuerSigningKeys = keySet.Keys,
            };

            try
            {
                _tokenHandler.ValidateToken(context.IdToken, parameters, out var _);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "SuperOffice ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}.",
                    parameters.ValidIssuer,
                    parameters.ValidAudience);

                _logger.LogTrace(
                    ex,
                    "SuperOffice ID token {IdToken} could not be validated.",
                    context.IdToken);

                throw;
            }
        }
    }
}
