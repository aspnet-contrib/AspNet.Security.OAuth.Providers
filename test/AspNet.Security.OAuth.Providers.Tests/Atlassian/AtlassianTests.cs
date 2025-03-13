// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using AspNet.Security.OAuth.AdobeIO;
using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Atlassian;

public class AtlassianTests(ITestOutputHelper outputHelper) : OAuthTests<AtlassianAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AtlassianAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddAtlassian(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "112233aa-bb11-cc22-33dd-445566abcabc")]
    [InlineData(ClaimTypes.Email, "mia@example.com")]
    [InlineData(ClaimTypes.Name, "Mia Krystof")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.AccountType, "atlassian")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Picture, "https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/112233aa-bb11-cc22-33dd-445566abcabc/1234abcd-9876-54aa-33aa-1234dfsade9487ds")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.AccountStatus, "active")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Nickname, "mkrystof")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.ZoneInfo, "Australia/Sydney")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Locale, "en-US")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.JobTitle, "Designer")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Organization, "mia@example.com")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Department, "Design team")]
    [InlineData(AtlassianOAuthenticationConstants.Claims.Location, "Sydney")]
    public async Task Can_Sign_In_Using_Atlassian(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task BuildChallengeUrl_Generates_Correct_Url()
    {
        // Arrange
        var options = new AtlassianAuthenticationOptions();

        var redirectUrl = "https://my-site.local/signin-atlassian";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new AtlassianAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://auth.atlassian.com/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKeyAndValue("audience", "api.atlassian.com");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("scope", "read:me");
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("prompt", "consent");
    }
}
