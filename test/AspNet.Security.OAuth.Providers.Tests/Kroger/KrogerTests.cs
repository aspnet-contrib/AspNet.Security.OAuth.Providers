/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Kroger;

public class KrogerTests(ITestOutputHelper outputHelper) : OAuthTests<KrogerAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => KrogerAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddKroger(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "53990804-cfd1-43f3-8256-bdc9817a4fd0")]
    public async Task Can_Sign_In_Using_Kroger(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
