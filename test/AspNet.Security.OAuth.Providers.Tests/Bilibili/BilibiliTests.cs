/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Bilibili;

public class BilibiliTests(ITestOutputHelper outputHelper) : OAuthTests<BilibiliAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => BilibiliAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddBilibili(options =>
        {
            ConfigureDefaults(builder, options);
        });
        LoopbackRedirectHandler.RedirectUri = "http://localhost/signin-bilibili";
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "9844422354fe42629cd126**********")]
    [InlineData("urn:bilibili:face", "https://i0.hdslb.com/bfs/face/member/noface.jpg")]
    [InlineData(ClaimTypes.Name, "TestAccount")]
    public async Task Can_Sign_In_Using_Bilibili(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task BuildChallengeUrl_Generates_Correct_Url()
    {
        // Arrange
        var options = new BilibiliAuthenticationOptions();

        var redirectUrl = "https://my-site.local/signin-bilibili";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new BilibiliAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://account.bilibili.com/pc/account-pc/auth/oauth");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("gourl", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");

        query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
        query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
    }
}
