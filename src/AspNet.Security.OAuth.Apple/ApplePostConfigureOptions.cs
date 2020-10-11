/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OAuth.Apple.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="AppleAuthenticationOptions"/>.
    /// </summary>
    public class ApplePostConfigureOptions : IPostConfigureOptions<AppleAuthenticationOptions>
    {
        private readonly IMemoryCache _cache;
        private readonly ISystemClock _clock;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplePostConfigureOptions"/> class.
        /// </summary>
        /// <param name="cache">The <see cref="IMemoryCache"/> to use.</param>
        /// <param name="clock">The <see cref="ISystemClock"/> to use.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
        public ApplePostConfigureOptions(
            IMemoryCache cache,
            ISystemClock clock,
            ILoggerFactory loggerFactory)
        {
            _cache = cache;
            _clock = clock;
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        }

        /// <inheritdoc/>
        public void PostConfigure(
            [NotNull] string name,
            [NotNull] AppleAuthenticationOptions options)
        {
            if (options.JwtSecurityTokenHandler is null)
            {
                options.JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            }

            // Use a custom CryptoProviderFactory so that keys are not cached and then disposed of, see below:
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1302
            var cryptoProviderFactory = new CryptoProviderFactory() { CacheSignatureProviders = false };

            if (options.KeyStore is null)
            {
                options.KeyStore = new DefaultAppleKeyStore(
                    _clock,
                    _loggerFactory.CreateLogger<DefaultAppleKeyStore>());
            }

            if (options.ClientSecretGenerator is null)
            {
                options.ClientSecretGenerator = new DefaultAppleClientSecretGenerator(
                    options.KeyStore,
                    _cache,
                    _clock,
                    cryptoProviderFactory,
                    _loggerFactory.CreateLogger<DefaultAppleClientSecretGenerator>());
            }

            if (options.TokenValidator is null)
            {
                options.TokenValidator = new DefaultAppleIdTokenValidator(
                    options.KeyStore,
                    cryptoProviderFactory,
                    _loggerFactory.CreateLogger<DefaultAppleIdTokenValidator>());
            }
        }
    }
}
