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

namespace AspNet.Security.OAuth.Weixin
{
    public class WechatTests : OAuthTests<WeixinAuthenticationOptions>
    {
        public WechatTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => WeixinAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddWeixin(options =>
            {
                ConfigureDefaults(builder, options);
                options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Gender, "Male")]
        [InlineData(ClaimTypes.Country, "CN")]
        [InlineData("urn:weixin:city", "Beijing")]
        [InlineData("urn:weixin:headimgurl", "https://weixin.qq.local/image.png")]
        [InlineData("urn:weixin:openid", "my-open-id")]
        [InlineData("urn:weixin:privilege", "a,b,c")]
        [InlineData("urn:weixin:province", "Hebei")]
        public async Task Can_Sign_In_Using_Wechat(string claimType, string claimValue)
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
