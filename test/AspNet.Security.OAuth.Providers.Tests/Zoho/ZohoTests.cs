/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zoho;

public class ZohoTests : OAuthTests<ZohoAuthenticationOptions>
{
    public ZohoTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        LoopbackRedirectHandler.LoopbackParameters.Add("location", "us");
    }

    public override string DefaultScheme => ZohoAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddZoho(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1234567890")]
    [InlineData(ClaimTypes.Name, "User Name")]
    [InlineData(ClaimTypes.Email, "testuser@example.com")]
    public async Task Can_Sign_In_Using_Zoho(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
