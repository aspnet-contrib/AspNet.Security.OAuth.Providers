/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.PingOne;

public static class PingOneAuthenticationOptionsTests
{
    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Not_Set()
    {
        // Arrange
        var options = new PingOneAuthenticationOptions()
        {
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://auth.pingone.local",
            UserInformationEndpoint = "https://auth.pingone.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentNullException>("AuthorizationEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Not_Set()
    {
        // Arrange
        var options = new PingOneAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://auth.pingone.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            UserInformationEndpoint = "https://auth.pingone.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentNullException>("TokenEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_UserInformationEndpoint_Not_Set()
    {
        // Arrange
        var options = new PingOneAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://auth.pingone.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://auth.pingone.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("UserInformationEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Does_Not_Throw_If_Uris_Are_Valid()
    {
        // Arrange
        var options = new PingOneAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://auth.pingone.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://auth.pingone.local",
            UserInformationEndpoint = "https://auth.pingone.local",
        };

        // Act (no Assert)
        options.Validate();
    }
}
