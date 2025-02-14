/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Kook.KookAuthenticationConstants;

namespace AspNet.Security.OAuth.Kook;

public class KookTests(ITestOutputHelper outputHelper) : OAuthTests<KookAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => KookAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddKook(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "364862")]
    [InlineData(ClaimTypes.Name, "test")]
    [InlineData(ClaimTypes.MobilePhone, "110****2333")]
    [InlineData(Claims.IdentifyNumber, "1670")]
    [InlineData(Claims.OperatingSystem, "iOS")]
    [InlineData(Claims.AvatarUrl, "https://example.com/assets/avatar.png/icon")]
    [InlineData(Claims.BannerUrl, "https://example.com/assets/banner.png/icon")]
    [InlineData(Claims.IsMobileVerified, "True")]
    public async Task Can_Sign_In_Using_Kook(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
