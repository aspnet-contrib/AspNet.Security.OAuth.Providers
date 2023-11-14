/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Weixin;

public class WeixinTests(ITestOutputHelper outputHelper) : OAuthTests<WeixinAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => WeixinAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddWeixin(options => ConfigureDefaults(builder, options));
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
    public async Task Can_Sign_In_Using_Weixin(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

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
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<WeixinAuthenticationOptions>((options) =>
            {
                options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-open-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Gender, "Male")]
    [InlineData(ClaimTypes.Country, "CN")]
    [InlineData("urn:weixin:city", "Beijing")]
    [InlineData("urn:weixin:headimgurl", "https://weixin.qq.local/image.png")]
    [InlineData("urn:weixin:openid", "my-open-id")]
    [InlineData("urn:weixin:privilege", "a,b,c")]
    [InlineData("urn:weixin:province", "Hebei")]
    public async Task Can_Sign_In_Using_Weixin_With_No_Union_Id(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<WeixinAuthenticationOptions>((options) =>
            {
                options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
                options.UserInformationEndpoint = "https://api.weixin.qq.com/sns/userinfo/nounionid";
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new WeixinAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-weixin";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new WeixinAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://open.weixin.qq.com/connect/qrconnect?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("appid", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "snsapi_login,snsapi_userinfo");

        if (usePkce)
        {
            query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
        else
        {
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
    }
}
