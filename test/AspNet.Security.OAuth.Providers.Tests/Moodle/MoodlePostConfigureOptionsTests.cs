/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Moodle;

public static class MoodlePostConfigureOptionsTests
{
    [Theory]
    [InlineData("moodle.local")]
    [InlineData("http://moodle.local")]
    [InlineData("http://moodle.local/")]
    [InlineData("https://moodle.local")]
    [InlineData("https://moodle.local/")]
    public static void PostConfigure_Configures_Valid_Endpoints(string domain)
    {
        // Arrange
        var name = "Moodle";
        var target = new MoodlePostConfigureOptions();

        var options = new MoodleAuthenticationOptions()
        {
            Domain = domain,
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldStartWith("https://moodle.local/local/oauth/login.php");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldStartWith("https://moodle.local/local/oauth/token.php");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldStartWith("https://moodle.local/local/oauth/user_info.php");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public static void PostConfigure_Throws_If_Domain_Is_Invalid(string? value)
    {
        // Arrange
        var name = "Moodle";
        var target = new MoodlePostConfigureOptions();

        var options = new MoodleAuthenticationOptions()
        {
            Domain = value,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
    }
}
