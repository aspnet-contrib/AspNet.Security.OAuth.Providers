/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Weixin;

public class WeixinTests : OAuthTests<WeixinAuthenticationOptions>
{
    public WeixinTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

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
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
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
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<WeixinAuthenticationOptions>((options) =>
            {
                options.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
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

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
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
            (options, loggerFactory, encoder, clock) => new WeixinAuthenticationHandler(options, loggerFactory, encoder, clock));

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
