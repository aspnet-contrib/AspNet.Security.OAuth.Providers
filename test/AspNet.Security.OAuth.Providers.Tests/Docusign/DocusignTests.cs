/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Docusign;

public class DocusignTests(ITestOutputHelper outputHelper) : OAuthTests<DocusignAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => DocusignAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddDocusign(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.Name, "User Name")]
    [InlineData(ClaimTypes.Email, "testuser@example.com")]
    [InlineData(ClaimTypes.GivenName, "User")]
    [InlineData(ClaimTypes.Surname, "Name")]
    public async Task Can_Sign_In_Using_Docusign(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
