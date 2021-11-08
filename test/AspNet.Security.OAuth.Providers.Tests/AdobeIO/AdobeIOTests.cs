/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.AdobeIO
{
    public class AdobeIOTests : OAuthTests<AdobeIOAuthenticationOptions>
    {
        public AdobeIOTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AdobeIOAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddAdobeIO(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "B0DC108C5CD449CA0A494133@c62f24cc5b5b7e0e0a494004")]
        [InlineData(ClaimTypes.Name, "John Sample")]
        [InlineData(ClaimTypes.Email, "jsample@email.com")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Sample")]
        [InlineData(ClaimTypes.Country, "US")]
        [InlineData("urn:adobeio:account_type", "ent")]
        [InlineData("urn:adobeio:email_verified", "True")]
        public async Task Can_Sign_In_Using_AdobeIO(string claimType, string claimValue)
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
