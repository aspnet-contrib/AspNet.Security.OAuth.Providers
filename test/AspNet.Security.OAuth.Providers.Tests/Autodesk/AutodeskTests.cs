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

namespace AspNet.Security.OAuth.Autodesk
{
    public class AutodeskTests : OAuthTests<AutodeskAuthenticationOptions>
    {
        public AutodeskTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AutodeskAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddAutodesk(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData("urn:autodesk:emailverified", "True")]
        [InlineData("urn:autodesk:twofactorenabled", "False")]
        public async Task Can_Sign_In_Using_Autodesk(string claimType, string claimValue)
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
