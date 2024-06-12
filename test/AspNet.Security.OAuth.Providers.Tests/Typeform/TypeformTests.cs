/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Typeform;

public class TypeformTests(ITestOutputHelper outputHelper) : OAuthTests<TypeformAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => TypeformAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddTypeform(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "id")]
    [InlineData(ClaimTypes.Name, "Test Name")]
    [InlineData(ClaimTypes.Email, "testuser@example.com")]
    public async Task Can_Sign_In_Using_Typeform(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
