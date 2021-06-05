/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleClientSecretGenerator : AppleClientSecretGenerator
    {
        private readonly IMemoryCache _cache;
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;
        private readonly CryptoProviderFactory _cryptoProviderFactory;

        public DefaultAppleClientSecretGenerator(
            [NotNull] IMemoryCache cache,
            [NotNull] ISystemClock clock,
            [NotNull] CryptoProviderFactory cryptoProviderFactory,
            [NotNull] ILogger<DefaultAppleClientSecretGenerator> logger)
        {
            _cache = cache;
            _clock = clock;
            _cryptoProviderFactory = cryptoProviderFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<string> GenerateAsync([NotNull] AppleGenerateClientSecretContext context)
        {
            string key = CreateCacheKey(context.Options);

            return await _cache.GetOrCreateAsync(key, async (entry) =>
            {
                try
                {
                    (string clientSecret, DateTimeOffset expiresAt) = await GenerateNewSecretAsync(context);
                    entry.AbsoluteExpiration = expiresAt;
                    return clientSecret;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to generate new client secret for the {SchemeName} authentication scheme.",
                        context.Scheme.Name);

                    throw;
                }
            });
        }

        private static string CreateCacheKey(AppleAuthenticationOptions options)
        {
            var segments = new[]
            {
                nameof(DefaultAppleClientSecretGenerator),
                "ClientSecret",
                options.TeamId,
                options.ClientId,
                options.KeyId
            };

            return string.Join('+', segments);
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

            byte[] keyBlob = await context.Options.PrivateKeyBytes!(context.Options.KeyId!, context.HttpContext.RequestAborted);
            string clientSecret;

            using (var algorithm = CreateAlgorithm(keyBlob))
            {
                tokenDescriptor.SigningCredentials = CreateSigningCredentials(context.Options.KeyId!, algorithm);

                clientSecret = context.Options.SecurityTokenHandler.CreateToken(tokenDescriptor);
            }

            _logger.LogTrace("Generated new client secret with value {ClientSecret}.", clientSecret);

            return (clientSecret, expiresAt);
        }

        private static ECDsa CreateAlgorithm(byte[] keyBlob)
        {
            var algorithm = ECDsa.Create();

            try
            {
                algorithm.ImportPkcs8PrivateKey(keyBlob, out _);
                return algorithm;
            }
            catch (Exception)
            {
                algorithm?.Dispose();
                throw;
            }
        }

        private SigningCredentials CreateSigningCredentials(string keyId, ECDsa algorithm)
        {
            var key = new ECDsaSecurityKey(algorithm) { KeyId = keyId };

            // Use a custom CryptoProviderFactory so that keys are not cached and then disposed of, see below:
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1302
            return new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256Signature)
            {
                CryptoProviderFactory = _cryptoProviderFactory,
            };
        }
    }
}
