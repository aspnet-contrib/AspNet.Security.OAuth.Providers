/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Zalo;

public class ZaloTests(ITestOutputHelper outputHelper) : OAuthTests<ZaloAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => ZaloAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddZalo(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "HoangITK")]
    [InlineData(ClaimTypes.Gender, "male")]
    [InlineData(ClaimTypes.DateOfBirth, "1987-02-01")]
    public async Task Can_Sign_In_Using_Zalo(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new ZaloAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-zalo";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new ZaloAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://oauth.zaloapp.com/v3/auth?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("app_id", options.ClientId);
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "scope-1 scope-2");

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
    }
}
