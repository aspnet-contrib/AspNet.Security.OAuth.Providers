/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Snapchat;

public class SnapchatTests(ITestOutputHelper outputHelper) : OAuthTests<SnapchatAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => SnapchatAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddSnapchat(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "id")]
    [InlineData(ClaimTypes.Name, "Test Testing")]
    [InlineData(ClaimTypes.Email, "test.testing@testmail.com")]
    [InlineData("member_status", "MEMBER")]
    [InlineData("organization_id", "id")]
    [InlineData("id", "id")]
    public async Task Can_Sign_In_Using_Snapchat(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
