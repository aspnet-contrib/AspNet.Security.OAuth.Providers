/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Notion;

public class NotionTests(ITestOutputHelper outputHelper) : OAuthTests<NotionAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => NotionAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddNotion(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData("urn:notion:workspace_name", "mif")]
    [InlineData("urn:notion:workspace_icon", "icon")]
    [InlineData("urn:notion:bot_id", "mybot")]
    public async Task Can_Sign_In_Using_Notion(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
