/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Feishu;

public class FeishuTests(ITestOutputHelper outputHelper) : OAuthTests<FeishuAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => FeishuAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddFeishu(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "test-open-id")]
    [InlineData(ClaimTypes.Name, "test-name")]
    [InlineData(FeishuAuthenticationConstants.Claims.UnionId, "test-union-id")]
    [InlineData(FeishuAuthenticationConstants.Claims.Avatar, "https://www.feishu.cn/avatar/icon_big")]
    [InlineData(FeishuAuthenticationConstants.Claims.AvatarBig, "https://www.feishu.cn/avatar/icon_big")]
    [InlineData(FeishuAuthenticationConstants.Claims.AvatarMiddle, "https://www.feishu.cn/avatar/icon_middle")]
    [InlineData(FeishuAuthenticationConstants.Claims.AvatarThumb, "https://www.feishu.cn/avatar/icon_thumb")]
    [InlineData(FeishuAuthenticationConstants.Claims.AvatarUrl, "https://www.feishu.cn/avatar/icon")]
    [InlineData(FeishuAuthenticationConstants.Claims.Email, "zhangsan@feishu.cn")]
    [InlineData(FeishuAuthenticationConstants.Claims.EmployeeNumber, "111222333")]
    [InlineData(FeishuAuthenticationConstants.Claims.EnglishName, "Lilei")]
    [InlineData(FeishuAuthenticationConstants.Claims.Mobile, "+86130xxxx0000")]
    [InlineData(FeishuAuthenticationConstants.Claims.Name, "test-name")]
    [InlineData(FeishuAuthenticationConstants.Claims.OpenId, "test-open-id")]
    [InlineData(FeishuAuthenticationConstants.Claims.Picture, "https://www.feishu.cn/avatar")]
    [InlineData(FeishuAuthenticationConstants.Claims.Sub, "ou_caecc734c2e3328a62489fe0648c4b98779515d3")]
    [InlineData(FeishuAuthenticationConstants.Claims.TenantKey, "736588c92lxf175d")]
    [InlineData(FeishuAuthenticationConstants.Claims.UserId, "5d9bdxxx")]
    public async Task Can_Sign_In_Using_Feishu(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
