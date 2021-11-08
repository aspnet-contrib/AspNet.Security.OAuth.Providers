/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Lichess;

public class LichessTests : OAuthTests<LichessAuthenticationOptions>
{
    public LichessTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => LichessAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddLichess(options =>
        {
            ConfigureDefaults(builder, options);
            options.Scope.Add(LichessAuthenticationConstants.Scopes.EmailRead);
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "georges")]
    [InlineData(ClaimTypes.Name, "Georges")]
    [InlineData(ClaimTypes.Email, "abathur@mail.org")]
    [InlineData(ClaimTypes.GivenName, "Thibault")]
    [InlineData(ClaimTypes.Surname, "Duplessis")]
    public async Task Can_Sign_In_Using_Lichess(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
