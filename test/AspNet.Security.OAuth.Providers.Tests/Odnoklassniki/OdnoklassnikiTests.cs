/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Odnoklassniki;

public class OdnoklassnikiTests : OAuthTests<OdnoklassnikiAuthenticationOptions>
{
    public OdnoklassnikiTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => OdnoklassnikiAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddOdnoklassniki(options =>
        {
            ConfigureDefaults(builder, options);
            options.PublicSecret = "publicsecret";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "123456")]
    [InlineData(ClaimTypes.Name, "Vasya Ivanov")]
    [InlineData(ClaimTypes.Email, "some@email.ru")]
    [InlineData(ClaimTypes.Gender, "male")]
    [InlineData(ClaimTypes.Surname, "Ivanov")]
    [InlineData(ClaimTypes.GivenName, "Vasya")]
    [InlineData(ClaimTypes.DateOfBirth, "1998-12-08")]
    [InlineData(ClaimTypes.Locality, "ru")]
    [InlineData("urn:ok:image1", "https://i.mycdn.me/res/stub_50x50.gif")]
    [InlineData("urn:ok:image2", "https://i.mycdn.me/res/stub_128x96.gif")]
    [InlineData("urn:ok:image3", "https://i.mycdn.me/res/stub_128x96.gif")]
    public async Task Can_Sign_In_Using_Odnoklassniki(string claimType, string claimValue)
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
        var options = new OdnoklassnikiAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        options.Scope.Add("scope-1");

        var redirectUrl = "https://my-site.local/signin-odnoklassniki";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new OdnoklassnikiAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://connect.ok.ru/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "GET_EMAIL;scope-1");

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
