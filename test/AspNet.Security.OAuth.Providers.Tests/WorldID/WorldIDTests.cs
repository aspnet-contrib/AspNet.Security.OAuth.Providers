/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WorldID;

public class WorldIDTests(ITestOutputHelper outputHelper) : OAuthTests<WorldIDAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => WorldIDAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddWorldID(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "0x2ae86d6d747702b3b2c81811cd2b39875e8fa6b780ee4a207bdc203a7860b535")]
    [InlineData(ClaimTypes.Name, "World ID User")]
    [InlineData(ClaimTypes.Email, "0x2ae86d6d747702b3b2c81811cd2b39875e8fa6b780ee4a207bdc203a7860b535@id.worldcoin.org")]
    public async Task Can_Sign_In_Using_WorldID(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
