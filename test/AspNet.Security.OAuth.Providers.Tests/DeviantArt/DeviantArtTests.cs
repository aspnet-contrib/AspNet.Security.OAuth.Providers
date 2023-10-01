/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.DeviantArt;

public class DeviantArtTests(ITestOutputHelper outputHelper) : OAuthTests<DeviantArtAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => DeviantArtAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddDeviantArt(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData("urn:deviantart:name", "John Smith")]
    public async Task Can_Sign_In_Using_Deviant_Art(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
