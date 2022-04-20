/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Weixin;

public class WechatHasUnionIdTests : OAuthTests<WeixinAuthenticationOptions>
{
    public WechatHasUnionIdTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => "Wechat";

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddWeixin("Wechat", "Wechat", options =>
        {
            ConfigureDefaults(builder, options);
            options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
            options.CallbackPath = "/signin-wechat";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Gender, "Male")]
    [InlineData(ClaimTypes.Country, "CN")]
    [InlineData("urn:weixin:city", "Kunming")]
    [InlineData("urn:weixin:headimgurl", "https://weixin.qq.local/image.png")]
    [InlineData("urn:weixin:openid", "my-open-id")]
    [InlineData("urn:weixin:privilege", "a,b,c")]
    [InlineData("urn:weixin:province", "Yunnan")]
    [InlineData("urn:weixin:unionid", "my-id")]
    public async Task Can_Sign_In_Using_WechatPublic(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
