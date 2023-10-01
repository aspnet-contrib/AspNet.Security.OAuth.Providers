/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Line;

public class LineTests(ITestOutputHelper outputHelper) : OAuthTests<LineAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => LineAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddLine(options =>
        {
            ConfigureDefaults(builder, options);
            options.Scope.Add("email");
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-user-id")]
    [InlineData(ClaimTypes.Name, "my-display-name")]
    [InlineData("urn:line:picture_url", "my-picture")]
    [InlineData(ClaimTypes.Email, "my-email")]
    public async Task Can_Sign_In_Using_Line(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce, bool prompt)
    {
        // Arrange
        var options = new LineAuthenticationOptions()
        {
            Prompt = prompt,
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-line";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new LineAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://access.line.me/oauth2/v2.1/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "profile openid");

        if (usePkce)
        {
            query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
        else
        {
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
        }

        if (prompt)
        {
            query.ShouldContainKeyAndValue("prompt", "consent");
        }
        else
        {
            query.ShouldContainKeyAndValue("prompt", string.Empty);
        }
    }
}
