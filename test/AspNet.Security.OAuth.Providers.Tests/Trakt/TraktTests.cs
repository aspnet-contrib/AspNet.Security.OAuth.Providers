/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Trakt
{
    public class TraktTests : OAuthTests<TraktAuthenticationOptions>
    {
        public TraktTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => TraktAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddTrakt(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "sean")]
        [InlineData(ClaimTypes.Name, "Sean Rudford")]
        [InlineData("urn:trakt:vip", "True")]
        [InlineData("urn:trakt:vip_ep", "True")]
        [InlineData("urn:trakt:private", "False")]
        public async Task Can_Sign_In_Using_Trakt(string claimType, string claimValue)
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
