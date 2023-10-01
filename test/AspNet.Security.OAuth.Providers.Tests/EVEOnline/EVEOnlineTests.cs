/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.EVEOnline;

public class EVEOnlineTests(ITestOutputHelper outputHelper) : OAuthTests<EVEOnlineAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => EVEOnlineAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddEVEOnline(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Expiration, "2019-12-31T23:59:59+00:00")]
    [InlineData("urn:eveonline:scopes", "my-scopes")]
    public async Task Can_Sign_In_Using_EVE_Online(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
