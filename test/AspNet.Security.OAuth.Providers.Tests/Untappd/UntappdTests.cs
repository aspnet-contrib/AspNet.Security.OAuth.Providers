/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Untappd;

public class UntappdTests(ITestOutputHelper outputHelper) : OAuthTests<UntappdAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => UntappdAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddUntappd(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData(ClaimTypes.Webpage, "https://untappd.local/JohnSmith")]
    [InlineData("urn:untappd:link", "https://untappd.local/john-smith.png")]
    public async Task Can_Sign_In_Using_Untappd(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
