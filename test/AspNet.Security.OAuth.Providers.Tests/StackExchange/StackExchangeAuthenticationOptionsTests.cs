/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.StackExchange;

public static class StackExchangeAuthenticationOptionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public static void Validate_Throws_If_RequestKey_Is_Not_Set(string? value)
    {
        // Arrange
        var options = new StackExchangeAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            RequestKey = value!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("RequestKey", options.Validate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public static void Validate_Throws_If_Site_Is_Not_Set(string? value)
    {
        // Arrange
        var options = new StackExchangeAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            RequestKey = "my-request-key",
            Site = value!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("Site", options.Validate);
    }
}
