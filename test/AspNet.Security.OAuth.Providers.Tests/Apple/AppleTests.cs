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
                options.ClientSecret = string.Empty;
                options.GenerateClientSecret = true;
                options.KeyId = "my-key-id";
                options.TeamId = "my-team-id";
                options.ValidateTokens = true;
                options.PrivateKeyBytes = (keyId) =>
                {
                    Assert.Equal("my-key-id", keyId);
                    string privateKey = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgU208KCg/doqiSzsVF5sknVtYSgt8/3oiYGbvryIRrzSgCgYIKoZIzj0DAQehRANCAAQfrvDWizEnWAzB2Hx2r/NyvIBO6KGBDL7wkZoKnz4Sm4+1P1dhD9fVEhbsdoq9RKEf8dvzTOZMaC/iLqZFKSN6";
                    return Task.FromResult(Convert.FromBase64String(privateKey));
                };
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
        public async Task Can_Sign_In_Using_Apple(string claimType, string claimValue)
        {
            // Arrange
            using (var server = CreateTestServer((services) => services.AddSingleton<JwtSecurityTokenHandler, FrozenJwtSecurityTokenHandler>()))
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }

        private sealed class FrozenJwtSecurityTokenHandler : JwtSecurityTokenHandler
        {
            protected override void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
            {
                // Do not validate the lifetime as the token is old
            }
        }
    }
}
