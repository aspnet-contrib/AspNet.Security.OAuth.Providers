/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Smartsheet;

public class SmartsheetTests(ITestOutputHelper outputHelper) : OAuthTests<SmartsheetAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => SmartsheetAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddSmartsheet(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "12345")]
    [InlineData(ClaimTypes.GivenName, "Fred")]
    [InlineData(ClaimTypes.Surname, "Flintstone")]
    [InlineData(ClaimTypes.Email, "fred@example.com")]
    [InlineData(SmartsheetAuthenticationConstants.Claims.ProfileImage, "https://example.com/profile.png")]
    public async Task Can_Sign_In_Using_Smartsheet(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
