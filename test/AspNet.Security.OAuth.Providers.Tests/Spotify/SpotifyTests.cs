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

        public override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSpotify(options => ConfigureDefaults(builder, options));
        }

        [Fact]
        public async Task Can_Sign_In_Using_Spotify()
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
                    (ClaimTypes.NameIdentifier, "wizzler"),
                    (ClaimTypes.Name, "JM Wizzler"),
                    (ClaimTypes.Email, "email@example.com"),
                    (ClaimTypes.DateOfBirth, "1937-06-01"),
                    (ClaimTypes.Country, "SE"),
                    (ClaimTypes.Uri, "spotify:user:wizzler"),
                    ("urn:spotify:product", "premium"),
                    ("urn:spotify:url", "https://open.spotify.com/user/wizzler"));
            }
        }

        private void ConfigureTokenEndpoint()
            => ConfigureTokenEndpoint("https://accounts.spotify.com/api/token");

        private void ConfigureUserEndpoint()
        {
            // See https://developer.spotify.com/documentation/web-api/reference/users-profile/get-current-users-profile/
            ConfigureUserEndpoint(
                "https://api.spotify.com/v1/me",
                new
                {
                    birthdate = "1937-06-01",
                    country = "SE",
                    display_name = "JM Wizzler",
                    email = "email@example.com",
                    external_urls = new { spotify = "https://open.spotify.com/user/wizzler" },
                    followers = new { href = (string)null, total = 3829 },
                    href = "https://api.spotify.com/v1/users/wizzler",
                    images = new[]
                    {
                        new
                        {
                            height = (string)null,
                            url = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-frc3/t1.0-1/1970403_10152215092574354_1798272330_n.jpg",
                            width = (string)null
                        }
                    },
                    id = "wizzler",
                    product = "premium",
                    type = "user",
                    uri = "spotify:user:wizzler",
                });
        }
    }
}
