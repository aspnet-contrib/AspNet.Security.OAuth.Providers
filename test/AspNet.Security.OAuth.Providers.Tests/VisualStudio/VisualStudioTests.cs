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

namespace AspNet.Security.OAuth.VisualStudio
{
    public class VisualStudioTests : OAuthTests<VisualStudioAuthenticationOptions>
    {
        public VisualStudioTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => VisualStudioAuthenticationDefaults.AuthenticationScheme;

        public override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddVisualStudio(options => ConfigureDefaults(builder, options));
        }

        [Fact]
        public async Task Can_Sign_In_Using_Visual_Studio()
        {
            // Arrange
            ConfigureTokenEndpoint();
            ConfigureUserEndpoint();

            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaims(
                    claims,
                    (ClaimTypes.NameIdentifier, "my-id"),
                    (ClaimTypes.Name, "John Smith"),
                    (ClaimTypes.Email, "john@john-smith.local"),
                    (ClaimTypes.GivenName, "John"));
            }
        }

        private void ConfigureTokenEndpoint()
            => ConfigureTokenEndpoint("https://app.vssps.visualstudio.com/oauth2/token");

        private void ConfigureUserEndpoint()
        {
            ConfigureUserEndpoint(
                "https://app.vssps.visualstudio.com/_apis/profile/profiles/me",
                new
                {
                    id = "my-id",
                    publicAlias = "John Smith",
                    displayName = "John",
                    emailAddress = "john@john-smith.local",
                });
        }
    }
}
