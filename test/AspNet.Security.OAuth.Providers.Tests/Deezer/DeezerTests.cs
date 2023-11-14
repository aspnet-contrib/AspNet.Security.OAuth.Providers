/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Deezer;

public class DeezerTests(ITestOutputHelper outputHelper) : OAuthTests<DeezerAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => DeezerAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddDeezer(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Surname, "Smith")]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    [InlineData(ClaimTypes.DateOfBirth, "2020-01-01")]
    [InlineData(ClaimTypes.Gender, "male")]
    [InlineData(ClaimTypes.Country, "some country")]
    [InlineData(DeezerAuthenticationConstants.Claims.Username, "John Smith")]
    [InlineData(DeezerAuthenticationConstants.Claims.Avatar, "Avatar")]
    [InlineData(DeezerAuthenticationConstants.Claims.AvatarXL, "AvatarXL")]
    [InlineData(DeezerAuthenticationConstants.Claims.AvatarBig, "AvatarBig")]
    [InlineData(DeezerAuthenticationConstants.Claims.AvatarMedium, "AvatarMedium")]
    [InlineData(DeezerAuthenticationConstants.Claims.AvatarSmall, "AvatarSmall")]
    [InlineData(DeezerAuthenticationConstants.Claims.Url, "Url")]
    [InlineData(DeezerAuthenticationConstants.Claims.Status, "Status")]
    [InlineData(DeezerAuthenticationConstants.Claims.InscriptionDate, "InscriptionDate")]
    [InlineData(DeezerAuthenticationConstants.Claims.Language, "Language")]
    [InlineData(DeezerAuthenticationConstants.Claims.IsKid, "false")]
    [InlineData(DeezerAuthenticationConstants.Claims.Tracklist, "Tracklist")]
    [InlineData(DeezerAuthenticationConstants.Claims.Type, "Type")]
    [InlineData(DeezerAuthenticationConstants.Claims.ExplicitContentLevel, "ExplicitContentLevel")]
    public async Task Can_Sign_In_Using_Deezer(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new DeezerAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        options.Scope.Add("scope-1");

        var redirectUrl = "https://my-site.local/signin-deezer";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new DeezerAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://connect.deezer.com/oauth/auth.php?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("app_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("perms", "basic_access,scope-1");

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
