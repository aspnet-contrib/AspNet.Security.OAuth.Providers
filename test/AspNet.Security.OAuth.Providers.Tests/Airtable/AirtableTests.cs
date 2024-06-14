/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Airtable;

public class AirtableTests(ITestOutputHelper outputHelper) : OAuthTests<AirtableAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AirtableAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddAirtable(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "usr3d5D4ghuiJ")]
    [InlineData(ClaimTypes.Email, "testuser@example.com")]
    public async Task Can_Sign_In_Using_Airtable(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
