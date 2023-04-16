/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.PingOne;

public class PingOneTests : OAuthTests<PingOneAuthenticationOptions>
{
    public PingOneTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => PingOneAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddPingOne(options =>
        {
            ConfigureDefaults(builder, options);
            options.EnvironmentId = "b775aadd-a2f3-4d88-a768-b7c85dd1af47";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "john.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Name, "John Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g3")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_PingOne(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "jane.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "Jane")]
    [InlineData(ClaimTypes.Name, "Jane Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g4")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_PingOne_With_Custom_Domain(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PingOneAuthenticationOptions>("PingOne", (options) =>
            {
                options.Domain = "auth.pingone.local";
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
