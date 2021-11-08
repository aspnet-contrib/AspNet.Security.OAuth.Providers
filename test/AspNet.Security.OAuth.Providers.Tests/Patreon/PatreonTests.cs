/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Patreon
{
    public class PatreonTests : OAuthTests<PatreonAuthenticationOptions>
    {
        public PatreonTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => PatreonAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddPatreon(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "john.smith@patreon.local")]
        [InlineData(ClaimTypes.NameIdentifier, "12345")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData(ClaimTypes.Webpage, "https://patreon.local/JohnSmith")]
        [InlineData("urn:patreon:avatar", "https://patreon.local/JohnSmith/avatar.png")]
        public async Task Can_Sign_In_Using_Patreon(string claimType, string claimValue)
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
