/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Pipedrive;

public class PipedriveTests(ITestOutputHelper outputHelper) : OAuthTests<PipedriveAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => PipedriveAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddPipedrive(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "12345")]
    [InlineData(ClaimTypes.Name, "Test User")]
    [InlineData(ClaimTypes.Email, "testuser@example.com")]
    public async Task Can_Sign_In_Using_Pipedrive(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
