/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.AmoCrm;

public class AmoCrmTests(ITestOutputHelper outputHelper) : OAuthTests<AmoCrmAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AmoCrmAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddAmoCrm(options =>
        {
            options.Account = "example";
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "100500")]
    [InlineData(ClaimTypes.Name, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    public async Task Can_Sign_In_Using_AmoCrm(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
