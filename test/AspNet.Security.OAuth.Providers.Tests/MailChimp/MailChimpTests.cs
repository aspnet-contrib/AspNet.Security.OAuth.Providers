/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.MailChimp;

public class MailChimpTests(ITestOutputHelper outputHelper) : OAuthTests<MailChimpAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => MailChimpAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddMailChimp(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    public async Task Can_Sign_In_Using_MailChimp(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
