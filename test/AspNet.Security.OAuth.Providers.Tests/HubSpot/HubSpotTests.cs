/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.HubSpot.HubSpotAuthenticationConstants;

namespace AspNet.Security.OAuth.HubSpot;

public class HubSpotTests : OAuthTests<HubSpotAuthenticationOptions>
{
    public HubSpotTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => HubSpotAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddHubSpot(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "dummy@dummymail.com")]
    [InlineData(ClaimTypes.Email, "dummy@dummymail.com")]
    [InlineData(Claims.HubId, "13371337")]
    [InlineData(Claims.UserId, "123123")]
    [InlineData(Claims.AppId, "696969")]
    [InlineData(Claims.HubDomain, "dev-13371337.com")]

    public async Task Can_Sign_In_Using_HubSpot(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
