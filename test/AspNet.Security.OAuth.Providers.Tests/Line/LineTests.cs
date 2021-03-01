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

namespace AspNet.Security.OAuth.Line
{
    public class LineTests : OAuthTests<LineAuthenticationOptions>
    {
        public override string DefaultScheme => LineAuthenticationDefaults.AuthenticationScheme;

        public LineTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddLine(options =>
            {
                ConfigureDefaults(builder, options);
                options.Scope.Add("email");
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-user-id")]
        [InlineData(ClaimTypes.Name, "my-display-name")]
        [InlineData("urn:line:picture_url", "my-picture")]
        [InlineData(ClaimTypes.Email, "my-email")]
        public async Task Can_Sign_In_Using_Line(string claimType, string claimValue)
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
