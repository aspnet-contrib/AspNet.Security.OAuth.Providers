/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.BungieNet;

public class BungieNetTests : OAuthTests<BungieNetAuthenticationOptions>
{
    public BungieNetTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => BungieNetAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

        builder.AddBungieNet(options =>
        {
            ConfigureDefaults(builder, options);

            options.ApiKey = "my-api-key";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "125")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(BungieNetAuthenticationConstants.Claims.UniqueName, "John Smith#125")]
    [InlineData(BungieNetAuthenticationConstants.Claims.ProfilePicturePath, "/img/profile/avatars/default_avatar.gif")]
    public async Task Can_Sign_In_Using_BungieNet(string claimType, string claimValue)
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
        var options = new BungieNetAuthenticationOptions
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-bungienet";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder, clock) => new BungieNetAuthenticationHandler(options, loggerFactory, encoder, clock));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://www.bungie.net/en/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");

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
