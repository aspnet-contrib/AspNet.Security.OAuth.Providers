/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.SoundCloud
{
    public class SoundCloudTests : OAuthTests<SoundCloudAuthenticationOptions>
    {
        public SoundCloudTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => SoundCloudAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSoundCloud(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Country, "GB")]
        [InlineData("urn:soundcloud:city", "London")]
        [InlineData("urn:soundcloud:fullname", "John Q Smith")]
        [InlineData("urn:soundcloud:profileurl", "https://soundcloud.local/JohnSmith")]
        public async Task Can_Sign_In_Using_SoundCloud(string claimType, string claimValue)
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
