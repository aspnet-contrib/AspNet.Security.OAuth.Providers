/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.JumpCloud;

public static class JumpCloudPostConfigureOptionsTests
{
    [Theory]
    [InlineData("jumpcloud.local")]
    [InlineData("http://jumpcloud.local")]
    [InlineData("http://jumpcloud.local/")]
    [InlineData("https://jumpcloud.local")]
    [InlineData("https://jumpcloud.local/")]
    public static void PostConfigure_Configures_Valid_Endpoints(string domain)
    {
        // Arrange
        var name = "JumpCloud";
        var target = new JumpCloudPostConfigureOptions();

        var options = new JumpCloudAuthenticationOptions()
        {
            Domain = domain,
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldStartWith("https://jumpcloud.local/oauth2/auth");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldStartWith("https://jumpcloud.local/oauth2/token");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldStartWith("https://jumpcloud.local/userinfo");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public static void PostConfigure_Throws_If_Domain_Is_Invalid(string? value)
    {
        // Arrange
        var name = "JumpCloud";
        var target = new JumpCloudPostConfigureOptions();

        var options = new JumpCloudAuthenticationOptions()
        {
            Domain = value,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
    }
}
