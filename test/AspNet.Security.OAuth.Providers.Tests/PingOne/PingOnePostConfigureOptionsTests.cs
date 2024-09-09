/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.PingOne;

public static class PingOnePostConfigureOptionsTests
{
    [Fact]
    public static void PostConfigure_Configures_Valid_Endpoints()
    {
        // Arrange
        var name = "PingOne";
        var target = new PingOnePostConfigureOptions();

        var options = new PingOneAuthenticationOptions()
        {
            EnvironmentId = "b775aadd-a2f3-4d88-a768-b7c85dd1af47"
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldStartWith("https://auth.pingone.com/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldStartWith("https://auth.pingone.com/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldStartWith("https://auth.pingone.com/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Fact]
    public static void PostConfigure_Configures_Valid_Endpoints_With_Custom_Domain()
    {
        // Arrange
        var name = "PingOne";
        var target = new PingOnePostConfigureOptions();

        var options = new PingOneAuthenticationOptions()
        {
            EnvironmentId = "b775aadd-a2f3-4d88-a768-b7c85dd1af47",
            Domain = "auth.pingone.local"
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldStartWith("https://auth.pingone.local/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldStartWith("https://auth.pingone.local/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldStartWith("https://auth.pingone.local/b775aadd-a2f3-4d88-a768-b7c85dd1af47/");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public static void PostConfigure_Throws_If_Domain_Is_Invalid(string? value)
    {
        // Arrange
        var name = "PingOne";
        var target = new PingOnePostConfigureOptions();

        var options = new PingOneAuthenticationOptions()
        {
            Domain = value!,
            EnvironmentId = "b775aadd-a2f3-4d88-a768-b7c85dd1af47",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public static void PostConfigure_Throws_If_EnvironmentId_Is_Invalid(string? value)
    {
        // Arrange
        var name = "PingOne";
        var target = new PingOnePostConfigureOptions();

        var options = new PingOneAuthenticationOptions()
        {
            EnvironmentId = value!,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
    }
}
