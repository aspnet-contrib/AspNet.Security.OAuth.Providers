/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Mixcloud;

public class MixcloudTests : OAuthTests<MixcloudAuthenticationOptions>
{
    public MixcloudTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => MixcloudAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddMixcloud(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John-Smith")]
    [InlineData(ClaimTypes.Country, "GB")]
    [InlineData("urn:mixcloud:biography", "I am John Smith.")]
    [InlineData("urn:mixcloud:city", "London")]
    [InlineData("urn:mixcloud:fullname", "John Q Smith")]
    [InlineData("urn:mixcloud:profileurl", "https://mixcloud.local/JohnSmith")]
    [InlineData("urn:mixcloud:profileimageurl", "https://mixcloud.local/images/320wx320h")]
    [InlineData("urn:mixcloud:profilethumbnailurl", "https://mixcloud.local/images/thumbnail")]
    public async Task Can_Sign_In_Using_Mixcloud(string claimType, string claimValue)
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
        var options = new MixcloudAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-mixcloud";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new MixcloudAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://www.mixcloud.com/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl + "?state=");
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
