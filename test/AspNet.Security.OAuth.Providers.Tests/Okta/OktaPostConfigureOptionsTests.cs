﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Shouldly;
using Xunit;

namespace AspNet.Security.OAuth.Okta
{
    public static class OktaPostConfigureOptionsTests
    {
        [Theory]
        [InlineData("okta.local")]
        [InlineData("http://okta.local")]
        [InlineData("http://okta.local/")]
        [InlineData("https://okta.local")]
        [InlineData("https://okta.local/")]
        public static void PostConfigure_Configures_Valid_Endpoints(string domain)
        {
            // Arrange
            string name = "Okta";
            var target = new OktaPostConfigureOptions();

            var options = new OktaAuthenticationOptions()
            {
                Domain = domain,
            };

            // Act
            target.PostConfigure(name, options);

            // Assert
            options.AuthorizationEndpoint.ShouldStartWith("https://okta.local/oauth2/default/v1/");
            Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.TokenEndpoint.ShouldStartWith("https://okta.local/oauth2/default/v1/");
            Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();

            options.UserInformationEndpoint.ShouldStartWith("https://okta.local/oauth2/default/v1/");
            Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out var _).ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public static void PostConfigure_Throws_If_Domain_Is_Invalid(string value)
        {
            // Arrange
            string name = "Okta";
            var target = new OktaPostConfigureOptions();

            var options = new OktaAuthenticationOptions()
            {
                Domain = value,
            };

            // Act and Assert
            Assert.Throws<ArgumentException>("options", () => target.PostConfigure(name, options));
        }
    }
}
