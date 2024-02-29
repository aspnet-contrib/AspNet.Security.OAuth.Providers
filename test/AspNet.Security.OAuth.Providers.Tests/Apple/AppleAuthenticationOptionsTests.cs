/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Apple;

public static class AppleAuthenticationOptionsTests
{
    [Fact]
    public static void Validate_Throws_If_ClientSecret_Is_Null_With_No_Secret_Generation()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = null!,
        };

        // Act and Assert
        Assert.Throws<ArgumentNullException>("ClientSecret", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Is_Null()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            AuthorizationEndpoint = null!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("AuthorizationEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Is_Null()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            TokenEndpoint = null!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("TokenEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_CallbackPath_Is_Null()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            CallbackPath = null,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("CallbackPath", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_KeyId_Is_Null_With_Secret_Generation()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            KeyId = null,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("KeyId", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TeamId_Is_Null_With_Secret_Generation()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            KeyId = "my-key-id",
            TeamId = null!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("TeamId", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TokenAudience_Is_Null_With_Secret_Generation()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            KeyId = "my-key-id",
            TeamId = "my-team-id",
            TokenAudience = null!,
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("TokenAudience", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_ClientSecretExpiresAfter_Is_Zero_With_Secret_Generation()
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            ClientId = "my-client-id",
            GenerateClientSecret = true,
            KeyId = "my-key-id",
            TeamId = "my-team-id",
            ClientSecretExpiresAfter = TimeSpan.Zero,
        };

        // Act and Assert
        Assert.Throws<ArgumentOutOfRangeException>("ClientSecretExpiresAfter", options.Validate);
    }
}
