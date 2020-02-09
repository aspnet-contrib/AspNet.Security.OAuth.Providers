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

namespace AspNet.Security.OAuth.Harvest
{
    public class HarvestTests : OAuthTests<HarvestAuthenticationOptions>
    {
        public HarvestTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => HarvestAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddHarvest(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1001")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData(ClaimTypes.Email, "john.smith@mail.com")]
        public async Task Can_Sign_In_Using_Harvest(string claimType, string claimValue)
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
