/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        protected override HttpMethod RedirectMethod => HttpMethod.Post;

        protected override IDictionary<string, string> RedirectParameters { get; } = new Dictionary<string, string>()
        {
            ["user"] = @"{""name"":{""firstName"":""Johnny"",""lastName"":""Appleseed""},""email"":""johnny.appleseed@apple.local""}",
        };

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            IdentityModelEventSource.ShowPII = true;

            builder.AddApple(options =>
            {
                ConfigureDefaults(builder, options);
                options.ClientId = "com.martincostello.signinwithapple.test.client";
                options.SaveTokens = true;
            });
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
        [InlineData(ClaimTypes.GivenName, "Johnny")]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        [InlineData(ClaimTypes.Surname, "Appleseed")]
        public async Task Can_Sign_In_Using_Apple_With_Client_Secret(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.GenerateClientSecret = false;
                    options.ClientSecret = "my-client-secret";
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
        [InlineData(ClaimTypes.GivenName, "Johnny")]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        [InlineData(ClaimTypes.Surname, "Appleseed")]
        public async Task Can_Sign_In_Using_Apple_With_Private_Key(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ClientSecret = string.Empty;
                    options.GenerateClientSecret = true;
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.KeyId = "my-key-id";
                    options.TeamId = "my-team-id";
                    options.ValidateTokens = true;
                    options.PrivateKeyBytes = async (keyId) =>
                    {
                        Assert.Equal("my-key-id", keyId);
                        return await TestKeys.GetPrivateKeyBytesAsync();
                    };
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData("at_hash", "eOy0y7XVexdkzc7uuDZiCQ")]
        [InlineData("aud", "com.martincostello.signinwithapple.test.client")]
        [InlineData("auth_time", "1587211556")]
        [InlineData("email", "ussckefuz6@privaterelay.appleid.com")]
        [InlineData("email_verified", "true")]
        [InlineData("exp", "1587212159")]
        [InlineData("iat", "1587211559")]
        [InlineData("iss", "https://appleid.apple.com")]
        [InlineData("is_private_email", "true")]
        [InlineData("nonce_supported", "true")]
        [InlineData("sub", "001883.fcc77ba97500402389df96821ad9c790.1517")]
        [InlineData(ClaimTypes.Email, "ussckefuz6@privaterelay.appleid.com")]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        public async Task Can_Sign_In_Using_Apple_And_Receive_Claims_From_Id_Token(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ClientSecret = "my-client-secret";
                    options.GenerateClientSecret = false;
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/email";
                    options.ValidateTokens = false;
                });
            }

            RedirectParameters.Clear(); // Simulate second sign in where user data is not returned

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
        [InlineData(ClaimTypes.GivenName, "Johnny")]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        [InlineData(ClaimTypes.Surname, "Appleseed")]
        public async Task Can_Sign_In_Using_Apple_With_No_Token_Validation(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ValidateTokens = false;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Expired_Token()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldBeOfType<SecurityTokenExpiredException>();
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Audience()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.ClientId = "my-team";
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldBeOfType<SecurityTokenInvalidAudienceException>();
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Issuer()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.TokenAudience = "https://apple.local";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldBeOfType<SecurityTokenInvalidIssuerException>();
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Signing_Key()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.PublicKeyEndpoint = "https://appleid.apple.local/auth/keys/invalid";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldBeOfType<SecurityTokenInvalidSignatureException>();
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Unknown_Signing_Key()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.JwtSecurityTokenHandler = new FrozenJwtSecurityTokenHandler();
                    options.PublicKeyEndpoint = "https://appleid.apple.local/auth/keys/none";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldBeOfType<SecurityTokenUnableToValidateException>();
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Null_Token()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/null";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldNotBeNull();
            exception.InnerException.ShouldBeOfType<InvalidOperationException>();
            exception.InnerException!.Message.ShouldNotBeNull();
            exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_Malformed_Token()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/malformed";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldNotBeNull();
            exception.InnerException.ShouldBeOfType<ArgumentException>();
            exception.InnerException!.Message.ShouldNotBeNull();
            exception.InnerException.Message.ShouldStartWith("IDX");
        }

        [Fact]
        public async Task Cannot_Sign_In_Using_Apple_With_No_Token()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.TokenEndpoint = "https://appleid.apple.local/auth/token/none";
                    options.ValidateTokens = true;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

            // Assert
            exception.InnerException.ShouldNotBeNull();
            exception.InnerException.ShouldBeOfType<InvalidOperationException>();
            exception.InnerException!.Message.ShouldNotBeNull();
            exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
        }

        [Fact]
        public async Task Apple_Public_Keys_Are_Cached()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.PublicKeyCacheLifetime = TimeSpan.FromMinutes(1);
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            var options = server.Services.GetRequiredService<IOptions<AppleAuthenticationOptions>>().Value;

            var context = new AppleValidateIdTokenContext(
                new DefaultHttpContext(),
                new AuthenticationScheme("apple", "Apple", typeof(AppleAuthenticationHandler)),
                options,
                "my-token");

            // Act
            byte[] actual1 = await options.KeyStore.LoadPublicKeysAsync(context);
            byte[] actual2 = await options.KeyStore.LoadPublicKeysAsync(context);

            // Assert
            actual1.ShouldNotBeNull();
            actual1.ShouldNotBeEmpty();
            actual1.ShouldBeSameAs(actual2);
        }

        [Fact]
        public async Task Apple_Public_Keys_Are_Reloaded_Once_Cache_Lieftime_Expires()
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
                {
                    options.PublicKeyCacheLifetime = TimeSpan.FromSeconds(0.25);
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            var options = server.Services.GetRequiredService<IOptions<AppleAuthenticationOptions>>().Value;

            var context = new AppleValidateIdTokenContext(
                new DefaultHttpContext(),
                new AuthenticationScheme("apple", "Apple", typeof(AppleAuthenticationHandler)),
                options,
                "my-token");

            // Act
            byte[] first = await options.KeyStore.LoadPublicKeysAsync(context);

            // Assert
            first.ShouldNotBeNull();
            first.ShouldNotBeEmpty();

            // Arrange
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Act
            byte[] second = await options.KeyStore.LoadPublicKeysAsync(context);

            // Assert
            second.ShouldNotBeNull();
            second.ShouldNotBeEmpty();
            first.ShouldNotBeSameAs(second);
        }
    }
}
