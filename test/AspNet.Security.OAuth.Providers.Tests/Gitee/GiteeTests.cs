/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Gitee
{
    public class GiteeTests : OAuthTests<GiteeAuthenticationOptions>
    {
        public GiteeTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => GiteeAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddGitee(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "testid")]
        [InlineData(ClaimTypes.Name, "loginname")]
        [InlineData(ClaimTypes.Email, "testgmail")]
        [InlineData("urn:gitee:name", "name")]
        [InlineData("urn:gitee:url", "https://gitee.com/api/v5/users/loginname")]
        public async Task Can_Sign_In_Using_Gitee(string claimType, string claimValue)
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
