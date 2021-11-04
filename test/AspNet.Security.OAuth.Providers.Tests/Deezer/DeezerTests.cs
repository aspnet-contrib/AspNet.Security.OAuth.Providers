/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Deezer
{
    public class DeezerTests : OAuthTests<DeezerAuthenticationOptions>
    {
        public DeezerTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => DeezerAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddDeezer(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.DateOfBirth, "2020-01-01")]
        [InlineData(ClaimTypes.Gender, "male")]
        [InlineData(ClaimTypes.Country, "some country")]
        [InlineData(DeezerAuthenticationConstants.Claims.Username, "John Smith")]
        [InlineData(DeezerAuthenticationConstants.Claims.Avatar, "Avatar")]
        [InlineData(DeezerAuthenticationConstants.Claims.AvatarXL, "AvatarXL")]
        [InlineData(DeezerAuthenticationConstants.Claims.AvatarBig, "AvatarBig")]
        [InlineData(DeezerAuthenticationConstants.Claims.AvatarMedium, "AvatarMedium")]
        [InlineData(DeezerAuthenticationConstants.Claims.AvatarSmall, "AvatarSmall")]
        [InlineData(DeezerAuthenticationConstants.Claims.Url, "Url")]
        [InlineData(DeezerAuthenticationConstants.Claims.Status, "Status")]
        [InlineData(DeezerAuthenticationConstants.Claims.InscriptionDate, "InscriptionDate")]
        [InlineData(DeezerAuthenticationConstants.Claims.Language, "Language")]
        [InlineData(DeezerAuthenticationConstants.Claims.IsKid, "false")]
        [InlineData(DeezerAuthenticationConstants.Claims.Tracklist, "Tracklist")]
        [InlineData(DeezerAuthenticationConstants.Claims.Type, "Type")]
        [InlineData(DeezerAuthenticationConstants.Claims.ExplicitContentLevel, "ExplicitContentLevel")]
        public async Task Can_Sign_In_Using_Deezer(string claimType, string claimValue)
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
