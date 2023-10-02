/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Huawei;

public class HuaweiTests(ITestOutputHelper outputHelper) : OAuthTests<HuaweiAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => HuaweiAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddHuawei(options =>
        {
            options.FetchNickname = true;

            options.Scope.Add("profile");
            options.Scope.Add("email");

            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "test-name-identifier")]
    [InlineData(ClaimTypes.Name, "test-display-name")]
    [InlineData(HuaweiAuthenticationConstants.Claims.Avatar, "test-head-picture-url.jpg")]
    [InlineData(ClaimTypes.Email, "test-email@test")]
    public async Task Can_Sign_In_Using_Huawei(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
