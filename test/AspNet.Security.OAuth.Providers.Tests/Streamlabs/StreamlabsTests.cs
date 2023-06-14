/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Streamlabs;

public class StreamlabsTests : OAuthTests<StreamlabsAuthenticationOptions>
{
    public StreamlabsTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => StreamlabsAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddStreamlabs(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "8798311")]
    [InlineData(ClaimTypes.Name, "sertay")]
    [InlineData("urn:streamlabs:displayname", "Sertay")]
    [InlineData("urn:streamlabs:primary", "twitch")]
    [InlineData("urn:streamlabs:thumbnail", "https://static-cdn.jtvnw.net/jtv_user_pictures/3bec2329-a591-40fe-852b-24b9eb88f351-profile_image-300x300.png")]
    [InlineData("urn:streamlabs:twitchiconurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/3bec2329-a591-40fe-852b-24b9eb88f351-profile_image-300x300.png")]
    [InlineData("urn:streamlabs:twitchid", "163981644")]
    [InlineData("urn:streamlabs:twitchname", "sertay")]
    [InlineData("urn:streamlabs:twitchdisplayname", "Sertay")]
    [InlineData("urn:streamlabs:facebookid", "15645645654645")]
    [InlineData("urn:streamlabs:facebookname", "Sertay")]
    [InlineData("urn:streamlabs:youtubeid", "UCC8H-NrBvYEqZ5KG-jzS_Oy")]
    [InlineData("urn:streamlabs:youtubetitle", "Sertay")]
    public async Task Can_Sign_In_Using_Streamlabs(string claimType, string claimValue)
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
        var options = new StreamlabsAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-streamlabs";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new StreamlabsAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://streamlabs.com/api/v1.0/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
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
