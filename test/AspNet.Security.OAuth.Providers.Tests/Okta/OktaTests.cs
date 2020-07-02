/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Okta
{
    public class OktaTests : OAuthTests<OktaAuthenticationOptions>
    {
        public OktaTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => OktaAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddOkta(options =>
            {
                ConfigureDefaults(builder, options);
                options.UseDomain("okta.local");
            });
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "john.doe@example.com")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Name, "John Doe")]
        [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g3")]
        [InlineData(ClaimTypes.Surname, "Doe")]
        public async Task Can_Sign_In_Using_Okta(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
