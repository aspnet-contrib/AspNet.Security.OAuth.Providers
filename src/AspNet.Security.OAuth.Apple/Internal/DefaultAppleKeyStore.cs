/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleKeyStore : AppleKeyStore
    {
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;

        private byte[] _publicKey = null!;
        private DateTimeOffset _reloadKeysAfter;

        public DefaultAppleKeyStore(
            [NotNull] ISystemClock clock,
            [NotNull] ILogger<DefaultAppleKeyStore> logger)
        {
            _clock = clock;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<byte[]> LoadPrivateKeyAsync([NotNull] AppleGenerateClientSecretContext context)
        {
            if (context.Options.PrivateKeyBytes == null)
            {
                throw new ArgumentException(
                    $"The {nameof(AppleAuthenticationOptions.PrivateKeyBytes)} option must be set to be able to load the Sign in with Apple private key.",
                    nameof(AppleAuthenticationOptions.PrivateKeyBytes));
            }

            return await context.Options.PrivateKeyBytes(context.Options.KeyId!);
        }

        /// <inheritdoc />
        public override async Task<byte[]> LoadPublicKeysAsync([NotNull] AppleValidateIdTokenContext context)
        {
            var utcNow = _clock.UtcNow;

            if (_publicKey == null || _reloadKeysAfter < utcNow)
            {
                _logger.LogInformation("Loading Apple public keys from {PublicKeyEndpoint}.", context.Options.PublicKeyEndpoint);

                _publicKey = await LoadApplePublicKeysAsync(context);
                _reloadKeysAfter = utcNow.Add(context.Options.PublicKeyCacheLifetime);

                _logger.LogInformation(
                    "Loaded Apple public keys from {PublicKeyEndpoint}. Keys will be reloaded at or after {ReloadKeysAfter}.",
                    context.Options.PublicKeyEndpoint,
                    _reloadKeysAfter);
            }

            return _publicKey;
        }

        private async Task<byte[]> LoadApplePublicKeysAsync([NotNull] AppleValidateIdTokenContext context)
        {
            var response = await context.Options.Backchannel.GetAsync(context.Options.PublicKeyEndpoint, context.HttpContext.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while retrieving the public keys from Apple: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.",
                                 /* Status: */ response.StatusCode,
                                 /* Headers: */ response.Headers.ToString(),
                                 /* Body: */ await response.Content.ReadAsStringAsync(context.HttpContext.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the public keys from Apple.");
            }

            return await response.Content.ReadAsByteArrayAsync(context.HttpContext.RequestAborted);
        }
    }
}
