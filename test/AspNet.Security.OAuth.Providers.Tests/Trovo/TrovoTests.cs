/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;
using static AspNet.Security.OAuth.Trovo.TrovoAuthenticationConstants;

namespace AspNet.Security.OAuth.Trovo;

public class TrovoTests : OAuthTests<TrovoAuthenticationOptions>
{
    public TrovoTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => TrovoAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddTrovo(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "100000021")]
    [InlineData(ClaimTypes.Name, "leafinsummer")]
    [InlineData(ClaimTypes.GivenName, "leafinsummer")]
    [InlineData(ClaimTypes.Email, "lxxxxxxxxx@gmail.com")]
    [InlineData(Claims.ProfilePic, "https://headicon.trovo.live/user/cxq7kbiaaaaabd5cniezh3x5cu.jpeg?t=2")]
    [InlineData(Claims.ChannelId, "100000021")]
    public async Task Can_Sign_In_Using_Trovo(string claimType, string claimValue)
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
        var options = new TrovoAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-trovo";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new TrovoAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://open.trovo.live/page/login.html");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "user_details_self");

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
