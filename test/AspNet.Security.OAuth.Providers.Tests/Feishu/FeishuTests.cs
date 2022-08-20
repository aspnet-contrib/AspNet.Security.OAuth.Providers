/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Feishu;

public class FeishuTests : OAuthTests<FeishuAuthenticationOptions>
{
    public FeishuTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => FeishuAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddFeishu(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "test-open-id")]
    [InlineData(ClaimTypes.Name, "test-name")]
    [InlineData(FeishuAuthenticationConstants.Claims.UnionId, "test-union-id")]
    [InlineData(FeishuAuthenticationConstants.Claims.Avatar, "https://www.feishu.cn/avatar/icon_big")]
    public async Task Can_Sign_In_Using_Feishu(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
