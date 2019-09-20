/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleKeyStore : AppleKeyStore
    {
        private readonly ILogger _logger;

        private byte[] _publicKey;

        public DefaultAppleKeyStore(
            [NotNull] ILogger<DefaultAppleKeyStore> logger)
        {
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

            return await context.Options.PrivateKeyBytes(context.Options.KeyId);
        }

        /// <inheritdoc />
        public override async Task<byte[]> LoadPublicKeysAsync([NotNull] AppleValidateIdTokenContext context)
        {
            if (_publicKey == null)
            {
                _publicKey = await LoadApplePublicKeysAsync(context);
            }

            return _publicKey;
        }

        private async Task<byte[]> LoadApplePublicKeysAsync([NotNull] AppleValidateIdTokenContext context)
        {
            _logger.LogInformation("Loading Apple public keys from {PublicKeyEndpoint}.", context.Options.PublicKeyEndpoint);

            var response = await context.Options.Backchannel.GetAsync(context.Options.PublicKeyEndpoint, context.HttpContext.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while retrieving the public keys from Apple: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.",
                                 /* Status: */ response.StatusCode,
                                 /* Headers: */ response.Headers.ToString(),
                                 /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the public keys from Apple.");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
