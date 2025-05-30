/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Webflow;

public class WebflowTests(ITestOutputHelper outputHelper) : OAuthTests<WebflowAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => WebflowAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddWebflow(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "545bbecb7bdd6769632504a7")]
    [InlineData(ClaimTypes.Email, "some@email.com")]
    [InlineData(ClaimTypes.GivenName, "Some")]
    [InlineData(ClaimTypes.Surname, "One")]
    public async Task Can_Sign_In_Using_Webflow(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
