/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Yahoo;

public class YahooTests : OAuthTests<YahooAuthenticationOptions>
{
    public YahooTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => YahooAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddYahoo(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "john@john-smith.local")]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData("urn:yahoo:familyname", "Smith")]
    [InlineData("urn:yahoo:givenname", "John")]
    [InlineData("urn:yahoo:picture", "https://www.yahoo.local/JohnSmith/image.png")]
    public async Task Can_Sign_In_Using_Yahoo(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
