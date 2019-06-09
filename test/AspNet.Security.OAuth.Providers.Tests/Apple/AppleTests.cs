/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Apple
{
    public class AppleTests : OAuthTests<AppleAuthenticationOptions>
    {
        public AppleTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AppleAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            IdentityModelEventSource.ShowPII = true;

            builder.AddApple(options =>
            {
                ConfigureDefaults(builder, options);
                options.ClientId = "com.martincostello.signinwithapple.test.client";
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        public async Task Can_Sign_In_Using_Apple_With_Client_Secret(string claimType, string claimValue)
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.GenerateClientSecret = false;
                    options.ClientSecret = "my-client-secret";
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        public async Task Can_Sign_In_Using_Apple_With_Private_Key(string claimType, string claimValue)
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ClientSecret = string.Empty;
                    options.GenerateClientSecret = true;
                    options.KeyId = "my-key-id";
                    options.TeamId = "my-team-id";
                    options.ValidateTokens = true;
                    options.PrivateKeyBytes = (keyId) =>
                    {
                        Assert.Equal("my-key-id", keyId);
                        return Task.FromResult(AppleClientSecretGeneratorTests.TestPrivateKey);
                    };
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        public async Task Can_Sign_In_Using_Apple_With_No_Token_Validation(string claimType, string claimValue)
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ValidateTokens = false;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Expired_Token()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<SecurityTokenExpiredException>();
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Audience()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ClientId = "my-team";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<SecurityTokenInvalidAudienceException>();
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Issuer()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenAudience = "https://apple.local";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<SecurityTokenInvalidIssuerException>();
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Signing_Key()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.PublicKeyEndpoint = "https://appleid.apple.local/auth/keys/invalid";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<SecurityTokenInvalidSignatureException>();
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Unknown_Signing_Key()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>();
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.PublicKeyEndpoint = "https://appleid.apple.local/auth/keys/none";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<SecurityTokenSignatureKeyNotFoundException>();
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Null_Token()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/null";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<InvalidOperationException>();
                exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Malformed_Token()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/malformed";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<ArgumentException>();
                exception.InnerException.Message.ShouldStartWith("IDX");
            }
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_No_Token()
        {
            // Arrange
            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/none";
                    options.ValidateTokens = true;
                });
            }

            using (var server = CreateTestServer(ConfigureServices))
            {
                // Act
                var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

                // Assert
                exception.InnerException.ShouldBeOfType<InvalidOperationException>();
                exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
            }
        }

        private sealed class FrozenJwtSecurityTokenHandler : JwtSecurityTokenHandler
        {
            protected override void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
            {
                // Do not validate the lifetime as the test token has expired
            }
        }
    }
}
