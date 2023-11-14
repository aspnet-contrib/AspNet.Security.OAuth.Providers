/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Trakt;

public class TraktTests(ITestOutputHelper outputHelper) : OAuthTests<TraktAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => TraktAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddTrakt(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "sean")]
    [InlineData(ClaimTypes.Name, "Sean Rudford")]
    [InlineData("urn:trakt:vip", "True")]
    [InlineData("urn:trakt:vip_ep", "True")]
    [InlineData("urn:trakt:private", "False")]
    public async Task Can_Sign_In_Using_Trakt(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
