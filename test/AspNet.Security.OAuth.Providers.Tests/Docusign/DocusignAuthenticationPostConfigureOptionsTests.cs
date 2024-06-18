/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Docusign;

public static class DocusignAuthenticationPostConfigureOptionsTests
{
    [Fact]
    public static void PostConfigure_Configures_Valid_Endpoints_For_Development_Environment()
    {
        // Arrange
        const string name = "Docusign";
        var target = new DocusignAuthenticationPostConfigureOptions();

        var options = new DocusignAuthenticationOptions
        {
            Environment = DocusignAuthenticationEnvironment.Development
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.DevelopmentDomain}{DocusignAuthenticationDefaults.AuthorizationPath}");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.DevelopmentDomain}{DocusignAuthenticationDefaults.TokenPath}");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.DevelopmentDomain}{DocusignAuthenticationDefaults.UserInformationPath}");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Fact]
    public static void PostConfigure_Configures_Valid_Endpoints_For_Production_Environment()
    {
        // Arrange
        const string name = "Docusign";
        var target = new DocusignAuthenticationPostConfigureOptions();

        var options = new DocusignAuthenticationOptions
        {
            Environment = DocusignAuthenticationEnvironment.Production
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.ProductionDomain}{DocusignAuthenticationDefaults.AuthorizationPath}");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.ProductionDomain}{DocusignAuthenticationDefaults.TokenPath}");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldBeEquivalentTo(
            $"https://{DocusignAuthenticationDefaults.ProductionDomain}{DocusignAuthenticationDefaults.UserInformationPath}");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Fact]
    public static void PostConfigure_InvalidEnvironment_ThrowsException()
    {
        // Arrange
        const string name = "Docusign";
        var target = new DocusignAuthenticationPostConfigureOptions();

        var options = new DocusignAuthenticationOptions
        {
            Environment = (DocusignAuthenticationEnvironment)3
        };

        // Act
        Action act = () => target.PostConfigure(name, options);
        act.ShouldThrow<InvalidOperationException>();
    }
}
