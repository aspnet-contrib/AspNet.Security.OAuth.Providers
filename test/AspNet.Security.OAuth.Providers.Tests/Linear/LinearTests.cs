/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Linear;

public class LinearTests(ITestOutputHelper outputHelper) : OAuthTests<LinearAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => LinearAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddLinear(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "the-user-id")]
    [InlineData(ClaimTypes.Name, "Marty McFly")]
    [InlineData(ClaimTypes.Email, "marty@hillvalley.com")]
    [InlineData(LinearAuthenticationConstants.Claims.OrganizationId, "the-org-id")]
    [InlineData(LinearAuthenticationConstants.Claims.OrganizationName, "Hill Valley Ventures")]
    [InlineData(LinearAuthenticationConstants.Claims.OrganizationUrlKey, "hill-valley")]
    public async Task Can_Sign_In_Using_Linear(string claimType, string claimValue)
    {
        await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
    }
}
