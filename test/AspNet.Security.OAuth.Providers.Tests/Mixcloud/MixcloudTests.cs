/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Mixcloud
{
    public class MixcloudTests : OAuthTests<MixcloudAuthenticationOptions>
    {
        public MixcloudTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => MixcloudAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddMixcloud(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Country, "GB")]
        [InlineData("urn:mixcloud:city", "London")]
        [InlineData("urn:mixcloud:name", "John Q Smith")]
        [InlineData("urn:mixcloud:profileurl", "https://soundcloud.local/JohnSmith")]
        [InlineData("urn:mixcloud:profileimageurl", "https://mixcloud.local/images/320wx320h")]
        [InlineData("urn:mixcloud:profilethumbnailurl", "https://mixcloud.local/images/thumbnail")]
        public async Task Can_Sign_In_Using_Mixcloud(string claimType, string claimValue)
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
