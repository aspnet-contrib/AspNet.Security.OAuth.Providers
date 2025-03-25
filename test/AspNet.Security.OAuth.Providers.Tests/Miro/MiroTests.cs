/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Miro;

public class MiroTests(ITestOutputHelper outputHelper) : OAuthTests<MiroAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => MiroAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddMiro(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "the-user-id")]
    [InlineData(ClaimTypes.Name, "John Wick")]
    [InlineData(MiroAuthenticationConstants.Claims.OrganizationId, "the-organization-id")]
    [InlineData(MiroAuthenticationConstants.Claims.OrganizationName, "Continental Services")]
    [InlineData(MiroAuthenticationConstants.Claims.TeamId, "the-team-id")]
    [InlineData(MiroAuthenticationConstants.Claims.TeamName, "Baba Yaga Unit")]
    public async Task Can_Sign_In_Using_Miro(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
