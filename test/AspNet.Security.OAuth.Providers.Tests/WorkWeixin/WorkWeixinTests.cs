/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WorkWeixin
{
    public class WorkWeixinTests : OAuthTests<WorkWeixinAuthenticationOptions>
    {
        public WorkWeixinTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => WorkWeixinAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddWorkWeixin(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-user-id")]
        [InlineData(ClaimTypes.Name, "wql")]
        [InlineData(ClaimTypes.Gender, "male")]
        [InlineData(ClaimTypes.Email, "wql1994@126.com")]
        [InlineData("urn:workweixin:avatar", "https://work.weixin.qq.local/avatar.png")]
        [InlineData("urn:workweixin:mobile", "888888")]
        [InlineData("urn:workweixin:alias", "my-alias")]
        public async Task Can_Sign_In_Using_WorkWeixin(string claimType, string claimValue)
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
