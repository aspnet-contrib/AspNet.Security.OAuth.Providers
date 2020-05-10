/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleClientSecretGenerator : AppleClientSecretGenerator
    {
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;
        private readonly AppleKeyStore _keyStore;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        private string? _clientSecret;
        private DateTimeOffset _expiresAt;

        public DefaultAppleClientSecretGenerator(
            [NotNull] AppleKeyStore keyStore,
            [NotNull] ISystemClock clock,
            [NotNull] JwtSecurityTokenHandler tokenHandler,
            [NotNull] ILogger<DefaultAppleClientSecretGenerator> logger)
        {
            _keyStore = keyStore;
            _clock = clock;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<string> GenerateAsync([NotNull] AppleGenerateClientSecretContext context)
        {
            if (_clientSecret == null || _clock.UtcNow >= _expiresAt)
            {
                try
                {
                    (_clientSecret, _expiresAt) = await GenerateNewSecretAsync(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to generate new client secret for the {context.Scheme.Name} authentication scheme.");
                    throw;
                }
            }

            return _clientSecret;
        }

        private async Task<(string clientSecret, DateTimeOffset expiresAt)> GenerateNewSecretAsync(
            [NotNull] AppleGenerateClientSecretContext context)
        {
            var expiresAt = _clock.UtcNow.Add(context.Options.ClientSecretExpiresAfter).UtcDateTime;
            var subject = new Claim("sub", context.Options.ClientId);

            _logger.LogDebug(
                "Generating new client secret for subject {Subject} that will expire at {ExpiresAt}.",
                subject.Value,
                expiresAt);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = context.Options.TokenAudience,
                Expires = expiresAt,
                Issuer = context.Options.TeamId,
                Subject = new ClaimsIdentity(new[] { subject }),
            };

            byte[] keyBlob = await _keyStore.LoadPrivateKeyAsync(context);
            string clientSecret;

            using (var algorithm = CreateAlgorithm(keyBlob))
            {
                tokenDescriptor.SigningCredentials = CreateSigningCredentials(context.Options.KeyId!, algorithm);

                clientSecret = _tokenHandler.CreateEncodedJwt(tokenDescriptor);
            }

            _logger.LogTrace("Generated new client secret with value {ClientSecret}.", clientSecret);

            return (clientSecret, expiresAt);
        }

        private static ECDsa CreateAlgorithm(byte[] keyBlob)
        {
            var algorithm = ECDsa.Create();

            try
            {
                algorithm.ImportPkcs8PrivateKey(keyBlob, out int _);
                return algorithm;
            }
            catch (Exception)
            {
                algorithm?.Dispose();
                throw;
            }
        }

        private static SigningCredentials CreateSigningCredentials(string keyId, ECDsa algorithm)
        {
            var key = new ECDsaSecurityKey(algorithm) { KeyId = keyId };
            return new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256Signature)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };
        }
    }
}
