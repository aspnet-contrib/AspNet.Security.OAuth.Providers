/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;
using Xunit;

namespace AspNet.Security.OAuth.Apple
{
    public static class AppleAuthenticationOptionsExtensionsTests
    {
        [Fact]
        public static void AddApple_Registers_Services()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddLogging()
                    .AddAuthentication()
                    .AddApple();

            // Act
            using var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetRequiredService<AppleAuthenticationHandler>().ShouldNotBeNull();
            serviceProvider.GetRequiredService<AppleClientSecretGenerator>().ShouldNotBeNull();
            serviceProvider.GetRequiredService<AppleIdTokenValidator>().ShouldNotBeNull();
            serviceProvider.GetRequiredService<AppleKeyStore>().ShouldNotBeNull();
            serviceProvider.GetRequiredService<IOptions<AppleAuthenticationOptions>>().ShouldNotBeNull();
        }

        [Fact]
        public static void AddApple_Does_Not_Overwrite_Existing_Service_Registrations()
        {
            // Arrange
            var cryptoProviderFactory = Mock.Of<CryptoProviderFactory>();
            var keyStore = Mock.Of<AppleKeyStore>();
            var secretGenerator = Mock.Of<AppleClientSecretGenerator>();
            var tokenValidator = Mock.Of<AppleIdTokenValidator>();

            var services = new ServiceCollection()
                .AddSingleton(cryptoProviderFactory)
                .AddSingleton(keyStore)
                .AddSingleton(secretGenerator)
                .AddSingleton(tokenValidator);

            services.AddLogging()
                    .AddAuthentication()
                    .AddApple();

            // Act
            using var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetRequiredService<AppleAuthenticationHandler>().ShouldNotBeNull();
            serviceProvider.GetRequiredService<IOptions<AppleAuthenticationOptions>>().ShouldNotBeNull();

            serviceProvider.GetRequiredService<AppleClientSecretGenerator>().ShouldBeSameAs(secretGenerator);
            serviceProvider.GetRequiredService<AppleIdTokenValidator>().ShouldBeSameAs(tokenValidator);
            serviceProvider.GetRequiredService<AppleKeyStore>().ShouldBeSameAs(keyStore);
            serviceProvider.GetRequiredService<CryptoProviderFactory>().ShouldBeSameAs(cryptoProviderFactory);
        }
    }
}
