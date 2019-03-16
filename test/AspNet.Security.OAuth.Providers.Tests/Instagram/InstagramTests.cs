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

namespace AspNet.Security.OAuth.Instagram
{
    public class InstagramTests : OAuthTests<InstagramAuthenticationOptions>
    {
        public InstagramTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => InstagramAuthenticationDefaults.AuthenticationScheme;

        public override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddInstagram(options => ConfigureDefaults(builder, options));
        }

        [Fact]
        public async Task Can_Sign_In_Using_Instagram()
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
                    (ClaimTypes.Name, "John Smith"));
            }
        }

        private void ConfigureTokenEndpoint()
            => ConfigureTokenEndpoint("https://api.instagram.com/oauth/access_token");

        private void ConfigureUserEndpoint()
        {
            ConfigureUserEndpoint(
                "https://api.instagram.com/v1/users/self?access_token=secret-access-token",
                new
                {
                    data = new
                    {
                        id = "my-id",
                        full_name = "John Smith",
                    }
                });
        }
    }
}
