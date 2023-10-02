/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitHub;

public class GitHubTests(ITestOutputHelper outputHelper) : OAuthTests<GitHubAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => GitHubAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddGitHub(options =>
        {
            ConfigureDefaults(builder, options);
            options.Scope.Add("user:email");
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1")]
    [InlineData(ClaimTypes.Name, "octocat")]
    [InlineData(ClaimTypes.Email, "octocat@github.com")]
    [InlineData("urn:github:name", "monalisa octocat")]
    [InlineData("urn:github:url", "https://api.github.com/users/octocat")]
    public async Task Can_Sign_In_Using_GitHub(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
