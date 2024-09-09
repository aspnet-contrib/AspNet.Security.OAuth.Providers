/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Keycloak;

public static class KeycloakAuthenticationOptionsTests
{
    public static TheoryData<KeycloakAuthenticationAccessType> AccessTypes => new()
    {
        { KeycloakAuthenticationAccessType.BearerOnly },
        { KeycloakAuthenticationAccessType.Confidential },
        { KeycloakAuthenticationAccessType.Public },
    };

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public static void Validate_Does_Not_Throw_If_ClientSecret_Is_Not_Provided_For_Public_Access_Type(string? clientSecret)
    {
        // Arrange
        var options = new KeycloakAuthenticationOptions()
        {
            AccessType = KeycloakAuthenticationAccessType.Public,
            ClientId = "my-client-id",
            ClientSecret = clientSecret!,
        };

        // Act (no Assert)
        options.Validate();
    }

    [Theory]
    [InlineData(KeycloakAuthenticationAccessType.BearerOnly)]
    [InlineData(KeycloakAuthenticationAccessType.Confidential)]
    public static void Validate_Throws_If_ClientSecret_Is_Null(KeycloakAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new KeycloakAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = null!,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>("ClientSecret", options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_AuthorizationEndpoint_Is_Null(KeycloakAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new KeycloakAuthenticationOptions()
        {
            AccessType = accessType,
            AuthorizationEndpoint = null!,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>("AuthorizationEndpoint", options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_TokenEndpoint_Is_Null(KeycloakAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new KeycloakAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            TokenEndpoint = null!,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>("TokenEndpoint", options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_CallbackPath_Is_Null(KeycloakAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new KeycloakAuthenticationOptions()
        {
            AccessType = accessType,
            CallbackPath = null,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("CallbackPath", options.Validate);
    }
}
