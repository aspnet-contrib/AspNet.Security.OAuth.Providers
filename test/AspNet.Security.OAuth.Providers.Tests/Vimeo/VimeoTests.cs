/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Vimeo;

public class VimeoTests(ITestOutputHelper outputHelper) : OAuthTests<VimeoAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => VimeoAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddVimeo(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData("urn:vimeo:fullname", "John Smith")]
    [InlineData("urn:vimeo:profileurl", "https://vimeo.local/JohnSmith")]
    public async Task Can_Sign_In_Using_Vimeo(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
