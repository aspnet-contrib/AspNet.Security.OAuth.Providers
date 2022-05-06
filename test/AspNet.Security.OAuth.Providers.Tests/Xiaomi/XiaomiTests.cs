/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Xiaomi;

public class XiaomiTests : OAuthTests<XiaomiAuthenticationOptions>
{
    public XiaomiTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => XiaomiAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddXiaomi(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-union-id")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    [InlineData("urn:xiaomi:miliaoIcon", "https://xiaomi.local/image.png")]
    [InlineData("urn:xiaomi:unionId", "my-union-id")]
    [InlineData("urn:xiaomi:miliaoNick", "John Smith")]
    public async Task Can_Sign_In_Using_Xiaomi(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
