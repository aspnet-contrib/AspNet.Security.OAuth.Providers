/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.GitLab;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.GitLab
{
    public class GitLabTests : OAuthTests<GitLabAuthenticationOptions>
    {
        public GitLabTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => GitLabAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddGitLab(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1")]
        [InlineData(ClaimTypes.Name, "testuser")]
        [InlineData(ClaimTypes.Email, "testuser@gitlab.com")]
        [InlineData(GitLabAuthenticationConstants.Claims.Name, "test user")]
        [InlineData(GitLabAuthenticationConstants.Claims.Url, "https://gitlab.com/testuser")]
        [InlineData(GitLabAuthenticationConstants.Claims.Avatar, "https://assets.gitlab-static.net/uploads/-/system/user/avatar/1234567/avatar.png")]
        public async Task Can_Sign_In_Using_GitHub(string claimType, string claimValue)
        {
            // Arrange
            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }
    }
}
