/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Paypal;

public class PaypalTests(ITestOutputHelper outputHelper) : OAuthTests<PaypalAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => PaypalAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddPaypal(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "mWq6_1sU85v5EG9yHdPxJRrhGHrnMJ-1PQKtX6pcsmA")]
    [InlineData(ClaimTypes.Name, "identity test")]
    [InlineData(ClaimTypes.Email, "user1@example.com")]
    [InlineData(ClaimTypes.GivenName, "identity")]
    [InlineData(ClaimTypes.Surname, "test")]
    public async Task Can_Sign_In_Using_Paypal(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
