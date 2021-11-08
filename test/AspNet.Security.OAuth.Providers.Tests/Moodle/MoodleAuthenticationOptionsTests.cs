﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Moodle;

public static class MoodleAuthenticationOptionsTests
{
    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Not_Set()
    {
        // Arrange
        var options = new MoodleAuthenticationOptions()
        {
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://moodle.local",
            UserInformationEndpoint = "https://moodle.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("AuthorizationEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Not_Set()
    {
        // Arrange
        var options = new MoodleAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://moodle.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            UserInformationEndpoint = "https://moodle.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("TokenEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Throws_If_UserInformationEndpoint_Not_Set()
    {
        // Arrange
        var options = new MoodleAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://moodle.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://moodle.local",
        };

        // Act and Assert
        Assert.Throws<ArgumentException>("UserInformationEndpoint", () => options.Validate());
    }

    [Fact]
    public static void Validate_Does_Not_Throw_If_Uris_Are_Valid()
    {
        // Arrange
        var options = new MoodleAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://moodle.local",
            ClientId = "ClientId",
            ClientSecret = "ClientSecret",
            TokenEndpoint = "https://moodle.local",
            UserInformationEndpoint = "https://moodle.local",
        };

        // Act (no Assert)
        options.Validate();
    }
}
