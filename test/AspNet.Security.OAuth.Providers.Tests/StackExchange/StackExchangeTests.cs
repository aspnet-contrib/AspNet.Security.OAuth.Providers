﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.StackExchange;

public class StackExchangeTests : OAuthTests<StackExchangeAuthenticationOptions>
{
    public StackExchangeTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => StackExchangeAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddStackExchange(options =>
        {
            ConfigureDefaults(builder, options);
            options.RequestKey = "request-key";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1")]
    [InlineData(ClaimTypes.Name, "Example User")]
    [InlineData(ClaimTypes.Webpage, "https://example.com/")]
    [InlineData("urn:stackexchange:link", "https://example.stackexchange.com/users/1/example-user")]
    public async Task Can_Sign_In_Using_StackExchange(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
