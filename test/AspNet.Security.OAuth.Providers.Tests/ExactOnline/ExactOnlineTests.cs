/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.ExactOnline.ExactOnlineAuthenticationConstants;

namespace AspNet.Security.OAuth.ExactOnline;

public class ExactOnlineTests(ITestOutputHelper outputHelper) : OAuthTests<ExactOnlineAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => ExactOnlineAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddExactOnline(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "00000000-0000-0000-0000-000000000000")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Email, "john@smith.org")]
    [InlineData(Claims.Division, "12345")]
    [InlineData(Claims.Company, "Division Name")]
    public async Task Can_Sign_In_Using_ExactOnline(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
