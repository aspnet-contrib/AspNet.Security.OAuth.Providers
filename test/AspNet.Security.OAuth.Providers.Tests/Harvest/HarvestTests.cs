/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Harvest;

public class HarvestTests(ITestOutputHelper outputHelper) : OAuthTests<HarvestAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => HarvestAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddHarvest(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1001")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData(ClaimTypes.Email, "john.smith@mail.com")]
    public async Task Can_Sign_In_Using_Harvest(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
