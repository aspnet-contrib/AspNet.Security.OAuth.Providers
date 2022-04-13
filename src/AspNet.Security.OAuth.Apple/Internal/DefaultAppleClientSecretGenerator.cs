﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal;

internal sealed partial class DefaultAppleClientSecretGenerator : AppleClientSecretGenerator
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

        var clientSecret = await _cache.GetOrCreateAsync(key, async (entry) =>
        {
            try
            {
                (string clientSecret, DateTimeOffset expiresAt) = await GenerateNewSecretAsync(context);
                entry.AbsoluteExpiration = expiresAt;
                return clientSecret;
            }
            catch (Exception ex)
            {
                Log.ClientSecretGenerationFailed(_logger, ex, context.Scheme.Name);
                throw;
            }
        });

        return clientSecret!;
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

    private async Task<(string ClientSecret, DateTimeOffset ExpiresAt)> GenerateNewSecretAsync(
        [NotNull] AppleGenerateClientSecretContext context)
    {
        var expiresAt = _clock.UtcNow.Add(context.Options.ClientSecretExpiresAfter).UtcDateTime;
        var subject = new Claim("sub", context.Options.ClientId);

        Log.GeneratingNewClientSecret(_logger, subject.Value, expiresAt);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = context.Options.TokenAudience,
            Expires = expiresAt,
            Issuer = context.Options.TeamId,
            Subject = new ClaimsIdentity(new[] { subject }),
        };

        var pem = await context.Options.PrivateKey!(context.Options.KeyId!, context.HttpContext.RequestAborted);
        string clientSecret;

        using (var algorithm = CreateAlgorithm(pem))
        {
            tokenDescriptor.SigningCredentials = CreateSigningCredentials(context.Options.KeyId!, algorithm);

            clientSecret = context.Options.SecurityTokenHandler.CreateToken(tokenDescriptor);
        }

        Log.GeneratedNewClientSecret(_logger, clientSecret);

        return (clientSecret, expiresAt);
    }

    private static ECDsa CreateAlgorithm(ReadOnlyMemory<char> pem)
    {
        var algorithm = ECDsa.Create();

        try
        {
            algorithm.ImportFromPem(pem.Span);
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
        return new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256)
        {
            CryptoProviderFactory = _cryptoProviderFactory,
        };
    }

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Error, "Failed to generate new client secret for the {SchemeName} authentication scheme.")]
        internal static partial void ClientSecretGenerationFailed(
            ILogger logger,
            Exception exception,
            string schemeName);

        [LoggerMessage(2, LogLevel.Debug, "Generating new client secret for subject {Subject} that will expire at {ExpiresAt}.")]
        internal static partial void GeneratingNewClientSecret(
            ILogger logger,
            string subject,
            DateTime expiresAt);

        [LoggerMessage(3, LogLevel.Trace, "Generated new client secret with value {ClientSecret}.")]
        internal static partial void GeneratedNewClientSecret(ILogger logger, string clientSecret);
    }
}
