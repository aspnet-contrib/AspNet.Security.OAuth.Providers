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

namespace AspNet.Security.OAuth.QQ
{
    public class QQTests : OAuthTests<QQAuthenticationOptions>
    {
        public QQTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => QQAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddQQ(options =>
            {
                options.ApplyForUnionID = true;
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Gender, "Male")]
        [InlineData("urn:qq:picture", "https://qq.local/picture.png")]
        [InlineData("urn:qq:picture_medium", "https://qq.local/picture-medium.png")]
        [InlineData("urn:qq:picture_full", "https://qq.local/picture-large.png")]
        [InlineData("urn:qq:avatar", "https://qq.local/avatar.png")]
        [InlineData("urn:qq:avatar_full", "https://qq.local/avatar-large.png")]
        [InlineData("urn:qq:unionid", "my-union-id")]
        public async Task Can_Sign_In_Using_QQ(string claimType, string claimValue)
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
