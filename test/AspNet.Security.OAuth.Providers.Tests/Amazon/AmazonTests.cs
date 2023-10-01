/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Amazon;

public class AmazonTests(ITestOutputHelper outputHelper) : OAuthTests<AmazonAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AmazonAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddAmazon(options =>
        {
            ConfigureDefaults(builder, options);
            options.Fields.Add("postal_code");
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    [InlineData(ClaimTypes.PostalCode, "90210")]
    public async Task Can_Sign_In_Using_Amazon(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
