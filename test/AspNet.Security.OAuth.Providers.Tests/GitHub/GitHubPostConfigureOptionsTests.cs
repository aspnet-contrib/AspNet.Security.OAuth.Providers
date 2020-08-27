/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Shouldly;
using Xunit;

namespace AspNet.Security.OAuth.GitHub
{
    public static class GitHubPostConfigureOptionsTests
    {
        [Theory]
        [InlineData("github.local")]
        [InlineData("http://github.local")]
        [InlineData("http://github.local/")]
        [InlineData("https://github.local")]
        [InlineData("https://github.local/")]
        public static void PostConfigure_Configures_Valid_Endpoints_For_GitHub_Enterprise(string value)
        {
            // Arrange
            string name = "GitHub";
            var target = new GitHubPostConfigureOptions();

            var options = new GitHubAuthenticationOptions()
            {
                EnterpriseDomain = value,
            };

            // Act
            target.PostConfigure(name, options);

            // Assert
            options.AuthorizationEndpoint.ShouldStartWith("https://github.local/");
            Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.TokenEndpoint.ShouldStartWith("https://github.local/");
            Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.UserEmailsEndpoint.ShouldStartWith("https://github.local/api/v3/");
            Uri.TryCreate(options.UserEmailsEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.UserInformationEndpoint.ShouldStartWith("https://github.local/api/v3/");
            Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public static void PostConfigure_Does_Not_Update_Endpoints_If_EnterpriseDomain_Not_Set(string value)
        {
            // Arrange
            string name = "GitHub";
            var target = new GitHubPostConfigureOptions();

            var options = new GitHubAuthenticationOptions()
            {
                EnterpriseDomain = value,
            };

            // Act
            target.PostConfigure(name, options);

            // Assert
            options.AuthorizationEndpoint.ShouldStartWith("https://github.com/");
            Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.TokenEndpoint.ShouldStartWith("https://github.com/");
            Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.UserEmailsEndpoint.ShouldStartWith("https://api.github.com/");
            Uri.TryCreate(options.UserEmailsEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.UserInformationEndpoint.ShouldStartWith("https://api.github.com/");
            Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();
        }
    }
}
