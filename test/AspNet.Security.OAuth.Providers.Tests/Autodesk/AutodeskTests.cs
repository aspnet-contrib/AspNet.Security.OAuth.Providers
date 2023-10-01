/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Autodesk;

public class AutodeskTests(ITestOutputHelper outputHelper) : OAuthTests<AutodeskAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AutodeskAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddAutodesk(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData("urn:autodesk:emailverified", "True")]
    public async Task Can_Sign_In_Using_Autodesk(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
