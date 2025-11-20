/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Douyin;

public class DouyinTests(ITestOutputHelper outputHelper) : OAuthTests<DouyinAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => DouyinAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddDouyin(options =>
        {
            ConfigureDefaults(builder, options);
            options.ClientSecret = "ee9ee51ee0ceabdeeeb9459168eeeef7";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "0da22181-d833-447f-995f-1beefe******")]
    [InlineData("urn:douyin:avatar", "https://example.com/x.jpeg")]
    [InlineData("urn:douyin:nickname", "TestAccount")]
    public async Task Can_Sign_In_Using_Douyin(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task BuildChallengeUrl_Generates_Correct_Url()
    {
        // Arrange
        var options = new DouyinAuthenticationOptions();

        var redirectUrl = "https://my-site.local/signin-douyin";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new DouyinAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://open.douyin.com/platform/oauth/connect/");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_key", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "user_info");

        query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
        query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
    }
}
