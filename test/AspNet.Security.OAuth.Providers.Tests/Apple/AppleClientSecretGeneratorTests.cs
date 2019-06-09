/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace AspNet.Security.OAuth.Apple
{
    public static class AppleClientSecretGeneratorTests
    {
        [Fact]
        public static async Task GenerateAsync_Generates_Valid_Signed_Jwt()
        {
            // Arrange
            var options = new AppleAuthenticationOptions()
            {
                ClientId = "my-client-id",
                ClientSecretExpiresAfter = TimeSpan.FromMinutes(1),
                KeyId = "my-key-id",
                TeamId = "my-team-id",
                PrivateKeyBytes = (keyId) => TestKeys.GetPrivateKeyBytesAsync(),
                PrivateKeyPassword = TestKeys.GetPrivateKeyPassword(),
            };

            await GenerateTokenAsync(options, async (generator, context) =>
            {
                var utcNow = DateTimeOffset.UtcNow;

                // Act
                string token = await generator.GenerateAsync(context);

                // Assert
                token.ShouldNotBeNullOrWhiteSpace();
                token.Count((c) => c == '.').ShouldBe(2); // Format: "{header}.{body}.{signature}"

                // Act
                var validator = new JwtSecurityTokenHandler();
                var securityToken = validator.ReadJwtToken(token);

                // Assert - See https://developer.apple.com/documentation/signinwithapplerestapi/generate_and_validate_tokens
                securityToken.ShouldNotBeNull();

                securityToken.Header.ShouldNotBeNull();
                securityToken.Header.ShouldContainKeyAndValue("alg", "ES256");
                securityToken.Header.ShouldContainKeyAndValue("kid", "my-key-id");

                securityToken.Payload.ShouldNotBeNull();
                securityToken.Payload.ShouldContainKey("exp");
                securityToken.Payload.ShouldContainKey("iat");
                securityToken.Payload.ShouldContainKeyAndValue("aud", "https://appleid.apple.com");
                securityToken.Payload.ShouldContainKeyAndValue("iss", "my-team-id");
                securityToken.Payload.ShouldContainKeyAndValue("sub", "my-client-id");

                ((long)securityToken.Payload.Iat.Value).ShouldBeGreaterThanOrEqualTo(utcNow.ToUnixTimeSeconds());
                ((long)securityToken.Payload.Exp.Value).ShouldBeGreaterThanOrEqualTo(utcNow.AddSeconds(60).ToUnixTimeSeconds());
                ((long)securityToken.Payload.Exp.Value).ShouldBeLessThanOrEqualTo(utcNow.AddSeconds(70).ToUnixTimeSeconds());
            });
        }

        [Fact]
        public static async Task GenerateAsync_Caches_Jwt_Until_Expired()
        {
            // Arrange
            var options = new AppleAuthenticationOptions()
            {
                ClientId = "my-client-id",
                ClientSecretExpiresAfter = TimeSpan.FromSeconds(1),
                KeyId = "my-key-id",
                TeamId = "my-team-id",
                PrivateKeyBytes = (keyId) => TestKeys.GetPrivateKeyBytesAsync(),
                PrivateKeyPassword = TestKeys.GetPrivateKeyPassword(),
            };

            await GenerateTokenAsync(options, async (generator, context) =>
            {
                // Act
                string token1 = await generator.GenerateAsync(context);
                string token2 = await generator.GenerateAsync(context);

                // Assert
                token2.ShouldBe(token1);

                // Act
                await Task.Delay(TimeSpan.FromSeconds(3));
                string token3 = await generator.GenerateAsync(context);

                // Assert
                token3.ShouldNotBe(token1);
            });
        }

        private static async Task GenerateTokenAsync(
            AppleAuthenticationOptions options,
            Func<AppleClientSecretGenerator, AppleGenerateClientSecretContext, Task> actAndAssert)
        {
            // Arrange
            var builder = new WebHostBuilder()
                .Configure((app) => app.UseAuthentication())
                .ConfigureServices((services) =>
                {
                    services.AddAuthentication()
                            .AddApple();
                });

            using (var host = builder.Build())
            {
                var httpContext = new DefaultHttpContext();
                var scheme = new AuthenticationScheme("Apple", "Apple", typeof(AppleAuthenticationHandler));

                var context = new AppleGenerateClientSecretContext(httpContext, scheme, options);
                var generator = host.Services.GetRequiredService<AppleClientSecretGenerator>();

                await actAndAssert(generator, context);
            }
        }
    }
}
