/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zendesk;

public class ZendeskTests : OAuthTests<ZendeskAuthenticationOptions>
{
    public ZendeskTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => ZendeskAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddZendesk(options =>
        {
            ConfigureDefaults(builder, options);
            options.Domain = "glowingwaffle.zendesk.com";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Name, "Johnny Agent")]
    [InlineData(ClaimTypes.NameIdentifier, "35436")]
    [InlineData(ClaimTypes.Email, "johnnyagent@zendesk.com")]
    public async Task Can_Sign_In_Using_Zendesk(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
