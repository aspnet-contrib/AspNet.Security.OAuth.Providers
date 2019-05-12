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

namespace AspNet.Security.OAuth.LinkedIn
{
    public class LinkedInTests : OAuthTests<LinkedInAuthenticationOptions>
    {
        public LinkedInTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => LinkedInAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddLinkedIn(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1R2RtA")]
        [InlineData(ClaimTypes.Name, "Frodo Baggins")]
        [InlineData(ClaimTypes.Email, "frodo@shire.middleearth")]
        [InlineData(ClaimTypes.GivenName, "Frodo")]
        [InlineData(ClaimTypes.Surname, "Baggins")]
        [InlineData("urn:linkedin:headline", "Jewelery Repossession in Middle Earth")]
        public async Task Can_Sign_In_Using_LinkedIn(string claimType, string claimValue)
        {
            // Arrange
            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }
    }
}
