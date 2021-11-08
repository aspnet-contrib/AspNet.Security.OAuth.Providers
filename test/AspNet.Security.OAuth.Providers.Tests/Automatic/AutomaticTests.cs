/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Automatic
{
    public class AutomaticTests : OAuthTests<AutomaticAuthenticationOptions>
    {
        public AutomaticTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AutomaticAuthenticationDefaults.AuthenticationScheme;

        protected override string RedirectUri => "http://localhost/signin-automatic";

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddAutomatic(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData(ClaimTypes.Uri, "https://automatic.local/JohnSmith")]
        public async Task Can_Sign_In_Using_Automatic(string claimType, string claimValue)
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
