/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Okta;

namespace AspNet.Security.OAuth.JumpCloud;

public static class JumpCloudAuthenticationOptionsTests
{
    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Not_Set()
    {
        // Arrange
        var options = new OktaAuthenticationOptions()
        {
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://jumpcloud.local",
            UserInformationEndpoint = "https://jumpcloud.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("AuthorizationEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Not_Set()
    {
        // Arrange
        var options = new OktaAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://jumpcloud.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            UserInformationEndpoint = "https://jumpcloud.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("TokenEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Throws_If_UserInformationEndpoint_Not_Set()
    {
        // Arrange
        var options = new OktaAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://okta.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://okta.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("UserInformationEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Does_Not_Throw_If_Uris_Are_Valid()
    {
        // Arrange
        var options = new OktaAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://okta.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://okta.local",
            UserInformationEndpoint = "https://okta.local",
        };

        // Act (no Assert)
        options.Validate();
    }
}
