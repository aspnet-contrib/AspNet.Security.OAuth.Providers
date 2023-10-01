/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.HubSpot;

public class HubSpotTests(ITestOutputHelper outputHelper) : OAuthTests<HubSpotAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => HubSpotAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddHubSpot(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "dummy@dummymail.com")]
    [InlineData(ClaimTypes.Email, "dummy@dummymail.com")]
    [InlineData("urn:HubSpot:hub_id", "13371337")]
    [InlineData("urn:HubSpot:user_id", "123123")]
    [InlineData("urn:HubSpot:app_id", "696969")]
    [InlineData("urn:HubSpot:hub_domain", "dev-13371337.com")]
    public async Task Can_Sign_In_Using_HubSpot(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
