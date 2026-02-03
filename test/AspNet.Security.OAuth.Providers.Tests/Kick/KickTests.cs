/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;
using static AspNet.Security.OAuth.Kick.KickAuthenticationConstants;

namespace AspNet.Security.OAuth.Kick;

public class KickTests(ITestOutputHelper outputHelper) : OAuthTests<KickAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => KickAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddKick(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "123456")]
    [InlineData(ClaimTypes.Name, "testuser")]
    [InlineData(ClaimTypes.Email, "test@example.com")]
    [InlineData(Claims.ProfilePicture, "https://files.kick.com/images/user/123456/profile_image.png")]
    public async Task Can_Sign_In_Using_Kick(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task BuildChallengeUrl_Generates_Correct_Url_With_Pkce()
    {
        // Arrange
        var options = new KickAuthenticationOptions();

        var redirectUrl = "https://my-site.local/signin-kick";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new KickAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://id.kick.com/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "user:read");

        // Kick requires PKCE
        query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
        query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
    }
}
