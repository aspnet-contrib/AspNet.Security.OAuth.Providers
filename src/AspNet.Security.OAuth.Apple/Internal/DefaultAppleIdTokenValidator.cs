/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleIdTokenValidator : AppleIdTokenValidator
    {
        private readonly ILogger _logger;
        private readonly AppleKeyStore _keyStore;
        private readonly CryptoProviderFactory _cryptoProviderFactory;

        public DefaultAppleIdTokenValidator(
            [NotNull] AppleKeyStore keyStore,
            [NotNull] CustomCryptoProviderFactory cryptoProviderFactory,
            [NotNull] ILogger<DefaultAppleIdTokenValidator> logger)
        {
            _keyStore = keyStore;
            _cryptoProviderFactory = cryptoProviderFactory;
            _logger = logger;
        }

        public override async Task ValidateAsync([NotNull] AppleValidateIdTokenContext context)
        {
            if (!context.Options.JwtSecurityTokenHandler!.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JwtSecurityTokenHandler)} cannot validate tokens.");
            }

            byte[] keysJson = await _keyStore.LoadPublicKeysAsync(context);

            string json = Encoding.UTF8.GetString(keysJson);
            var keySet = JsonWebKeySet.Create(json);

            var parameters = new TokenValidationParameters()
            {
                CryptoProviderFactory = _cryptoProviderFactory,
                IssuerSigningKeys = keySet.Keys,
                ValidAudience = context.Options.ClientId,
                ValidIssuer = context.Options.TokenAudience,
            };

            try
            {
                context.Options.JwtSecurityTokenHandler.ValidateToken(context.IdToken, parameters, out var _);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Apple ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}.",
                    parameters.ValidIssuer,
                    parameters.ValidAudience);

                _logger.LogTrace(
                    ex,
                    "Apple ID token {IdToken} could not be validated.",
                    context.IdToken);

                throw;
            }
        }
    }
}
