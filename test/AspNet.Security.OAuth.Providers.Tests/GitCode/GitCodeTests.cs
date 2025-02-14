/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitCode;

public class GitCodeTests(ITestOutputHelper outputHelper) : OAuthTests<GitCodeAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => GitCodeAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddGitCode(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "example-id")]
    [InlineData(ClaimTypes.Name, "example-login")]
    [InlineData(ClaimTypes.Email, "example@example.com")]
    [InlineData("urn:gitcode:avatar_url", "https://cdn-img.gitcode.com/fa/be/example.png?time=1694709764757")]
    [InlineData("urn:gitcode:bio", "Example bio")]
    [InlineData("urn:gitcode:blog", "https://gitcode.com")]
    [InlineData("urn:gitcode:company", "Example company")]
    [InlineData("urn:gitcode:html_url", "https://gitcode.com/example")]
    [InlineData("urn:gitcode:name", "example-name")]
    public async Task Can_Sign_In_Using_GitCode(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
