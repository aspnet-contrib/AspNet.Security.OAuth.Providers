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

namespace AspNet.Security.OAuth.Spotify
{
    public class SpotifyTests : OAuthTests<SpotifyAuthenticationOptions>
    {
        public SpotifyTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => SpotifyAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSpotify(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "wizzler")]
        [InlineData(ClaimTypes.Name, "JM Wizzler")]
        [InlineData(ClaimTypes.Email, "email@example.com")]
        [InlineData(ClaimTypes.DateOfBirth, "1937-06-01")]
        [InlineData(ClaimTypes.Country, "SE")]
        [InlineData(ClaimTypes.Uri, "spotify:user:wizzler")]
        [InlineData("urn:spotify:product", "premium")]
        [InlineData("urn:spotify:url", "https://open.spotify.com/user/wizzler")]
        [InlineData("urn:spotify:profilepicture", "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-frc3/t1.0-1/1970403_10152215092574354_1798272330_n.jpg")]
        public async Task Can_Sign_In_Using_Spotify(string claimType, string claimValue)
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
