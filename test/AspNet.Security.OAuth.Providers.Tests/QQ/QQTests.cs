/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.QQ;

public class QQTests : OAuthTests<QQAuthenticationOptions>
{
    public QQTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => QQAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddQQ(options =>
        {
            options.ApplyForUnionId = true;
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.Gender, "Male")]
    [InlineData("urn:qq:picture", "https://qq.local/picture.png")]
    [InlineData("urn:qq:picture_medium", "https://qq.local/picture-medium.png")]
    [InlineData("urn:qq:picture_full", "https://qq.local/picture-large.png")]
    [InlineData("urn:qq:avatar", "https://qq.local/avatar.png")]
    [InlineData("urn:qq:avatar_full", "https://qq.local/avatar-large.png")]
    [InlineData("urn:qq:unionid", "my-union-id")]
    public async Task Can_Sign_In_Using_QQ(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new QQAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        options.Scope.Add("scope-1");

        string redirectUrl = "https://my-site.local/signin-qq";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder, clock) => new QQAuthenticationHandler(options, loggerFactory, encoder, clock));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://graph.qq.com/oauth2.0/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "get_user_info,scope-1");

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
