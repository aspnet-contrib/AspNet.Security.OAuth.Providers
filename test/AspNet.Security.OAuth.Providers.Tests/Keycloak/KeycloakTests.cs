﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Keycloak
{
    public class KeycloakTests : OAuthTests<KeycloakAuthenticationOptions>
    {
        public KeycloakTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => KeycloakAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddKeycloak(options =>
            {
                ConfigureDefaults(builder, options);
                options.Domain = "keycloak.local";
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
        [InlineData(ClaimTypes.Email, "john@smith.com")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Role, "admin")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        public async Task Can_Sign_In_Using_Keycloak(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
