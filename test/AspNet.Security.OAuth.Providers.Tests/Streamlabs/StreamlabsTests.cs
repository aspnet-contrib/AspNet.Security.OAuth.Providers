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

namespace AspNet.Security.OAuth.Streamlabs
{
    public class StreamlabsTests : OAuthTests<StreamlabsAuthenticationOptions>
    {
        public StreamlabsTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => StreamlabsAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddStreamlabs(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "8798311")]
        [InlineData(ClaimTypes.Name, "sertay")]
        [InlineData("urn:streamlabs:displayname", "Sertay")]
        [InlineData("urn:streamlabs:primary", "twitch")]
        [InlineData("urn:streamlabs:thumbnail", "https://static-cdn.jtvnw.net/jtv_user_pictures/3bec2329-a591-40fe-852b-24b9eb88f351-profile_image-300x300.png")]
        [InlineData("urn:streamlabs:twitchiconurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/3bec2329-a591-40fe-852b-24b9eb88f351-profile_image-300x300.png")]
        [InlineData("urn:streamlabs:twitchid", "163981644")]
        [InlineData("urn:streamlabs:twitchname", "sertay")]
        [InlineData("urn:streamlabs:twitchdisplayname", "Sertay")]
        [InlineData("urn:streamlabs:facebookid", "15645645654645")]
        [InlineData("urn:streamlabs:facebookname", "Sertay")]
        [InlineData("urn:streamlabs:youtubeid", "UCC8H-NrBvYEqZ5KG-jzS_Oy")]
        [InlineData("urn:streamlabs:youtubetitle", "Sertay")]
        public async Task Can_Sign_In_Using_Streamlabs(string claimType, string claimValue)
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
