/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Twitch;

public class TwitchTests : OAuthTests<TwitchAuthenticationOptions>
{
    public TwitchTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => TwitchAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddTwitch(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "44322889")]
    [InlineData(ClaimTypes.Name, "dallas")]
    [InlineData(ClaimTypes.Email, "login@provider.com")]
    [InlineData("urn:twitch:broadcastertype", "Broadcaster")]
    [InlineData("urn:twitch:description", "Just a gamer playing games and chatting. :)")]
    [InlineData("urn:twitch:displayname", "Dallas")]
    [InlineData("urn:twitch:offlineimageurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-channel_offline_image-1a2c906ee2c35f12-1920x1080.png")]
    [InlineData("urn:twitch:profileimageurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-profile_image-1a2c906ee2c35f12-300x300.png")]
    [InlineData("urn:twitch:type", "staff")]
    public async Task Can_Sign_In_Using_Twitch(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce, bool forceVerify)
    {
        // Arrange
        var options = new TwitchAuthenticationOptions()
        {
            ForceVerify = forceVerify,
            UsePkce = usePkce,
        };

        options.Scope.Add("scope-1");

        var redirectUrl = "https://my-site.local/signin-twitch";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new TwitchAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://id.twitch.tv/oauth2/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "user:read:email scope-1");

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

        if (forceVerify)
        {
            query.ShouldContainKey("force_verify", "true");
        }
        else
        {
            query.ShouldContainKey("force_verify", "false");
        }
    }
}
