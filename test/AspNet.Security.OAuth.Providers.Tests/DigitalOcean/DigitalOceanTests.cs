/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.DigitalOcean;

public class DigitalOceanTests : OAuthTests<DigitalOceanAuthenticationOptions>
{
    public DigitalOceanTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => DigitalOceanAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddDigitalOcean(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Name, "me@test.com")]
    [InlineData(ClaimTypes.NameIdentifier, "b5d9f3d9-42b3-47e0-9413-8faab9895c69")]
    [InlineData(ClaimTypes.Email, "me@test.com")]
    public async Task Can_Sign_In_Using_DigitalOcean(string claimType, string claimValue)
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
        var options = new DigitalOceanAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        options.Scope.Add("read");

        string redirectUrl = "https://my-site.local/signin-digitalocean";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder, clock) => new DigitalOceanAuthenticationHandler(options, loggerFactory, encoder, clock));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://cloud.digitalocean.com/v1/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "read");

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
