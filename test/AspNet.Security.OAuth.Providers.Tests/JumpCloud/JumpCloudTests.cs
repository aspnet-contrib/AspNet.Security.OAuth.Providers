/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.JumpCloud;

public class JumpCloudTests : OAuthTests<JumpCloudAuthenticationOptions>
{
    public JumpCloudTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => JumpCloudAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddJumpCloud(options =>
        {
            ConfigureDefaults(builder, options);
            options.Domain = "jumpcloud.local";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "john.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Name, "John Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g3")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_JumpCloud(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
