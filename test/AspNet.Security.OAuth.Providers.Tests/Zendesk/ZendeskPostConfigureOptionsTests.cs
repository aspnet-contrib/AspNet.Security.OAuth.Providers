/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zendesk;

public static class ZendeskPostConfigureOptionsTests
{
    [Theory]
    [InlineData("glowingwaffle.zendesk.com")]
    [InlineData("http://glowingwaffle.zendesk.com")]
    [InlineData("http://glowingwaffle.zendesk.com/")]
    [InlineData("https://glowingwaffle.zendesk.com")]
    [InlineData("https://glowingwaffle.zendesk.com/")]
    public static void PostConfigure_Configures_Valid_Endpoints(string domain)
    {
        // Arrange
        var name = "Zendesk";
        var target = new ZendeskPostConfigureOptions();

        var options = new ZendeskAuthenticationOptions()
        {
            Domain = domain,
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldStartWith("https://glowingwaffle.zendesk.com/oauth/authorizations/new");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldStartWith("https://glowingwaffle.zendesk.com/oauth/tokens");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldStartWith("https://glowingwaffle.zendesk.com/api/v2/users/me");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public static void PostConfigure_Throws_If_Domain_Is_Invalid(string? value)
    {
        // Arrange
        var name = "Zendesk";
        var target = new ZendeskPostConfigureOptions();

        var options = new ZendeskAuthenticationOptions()
        {
            Domain = value,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
    }
}
