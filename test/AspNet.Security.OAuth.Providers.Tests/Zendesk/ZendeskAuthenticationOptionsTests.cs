/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zendesk;

public static class ZendeskAuthenticationOptionsTests
{
    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Not_Set()
    {
        // Arrange
        var options = new ZendeskAuthenticationOptions()
        {
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://glowingwaffle.zendesk.com",
            UserInformationEndpoint = "https://glowingwaffle.zendesk.com",
        };

        // Act and Assert
        Assert.Throws<ArgumentNullException>("AuthorizationEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Not_Set()
    {
        // Arrange
        var options = new ZendeskAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://glowingwaffle.zendesk.com",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            UserInformationEndpoint = "https://glowingwaffle.zendesk.com",
        };

        // Act and Assert
        Assert.Throws<ArgumentNullException>("TokenEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_UserInformationEndpoint_Not_Set()
    {
        // Arrange
        var options = new ZendeskAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://glowingwaffle.zendesk.com",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://glowingwaffle.zendesk.com",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("UserInformationEndpoint", options.Validate);
    }

    [Fact]
    public static void Validate_Does_Not_Throw_If_Uris_Are_Valid()
    {
        // Arrange
        var options = new ZendeskAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://glowingwaffle.zendesk.com",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://glowingwaffle.zendesk.com",
            UserInformationEndpoint = "https://glowingwaffle.zendesk.com",
        };

        // Act (no Assert)
        options.Validate();
    }
}
