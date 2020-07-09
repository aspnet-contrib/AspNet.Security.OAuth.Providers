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

namespace AspNet.Security.OAuth.GitHub
{
    public class GitHubEnterpriseTests : OAuthTests<GitHubAuthenticationOptions>
    {
        public GitHubEnterpriseTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => GitHubAuthenticationDefaults.AuthenticationScheme;

        protected override string BundleName => "GitHub";

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddGitHub(options =>
            {
                ConfigureDefaults(builder, options);
                options.EnterpriseDomain = "github.enterprise.local";
                options.Scope.Add("user:email");
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1")]
        [InlineData(ClaimTypes.Name, "octocat-work")]
        [InlineData(ClaimTypes.Email, "octocat@enterprise.local")]
        [InlineData("urn:github:name", "monalisa octocat")]
        [InlineData("urn:github:url", "https://api.github.enterprise.local/users/octocat")]
        public async Task Can_Sign_In_Using_GitHub_Enterprise(string claimType, string claimValue)
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
