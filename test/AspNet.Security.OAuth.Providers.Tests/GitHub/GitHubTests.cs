/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using JustEat.HttpClientInterception;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.GitHub
{
    public class GitHubTests : OAuthTests<GitHubAuthenticationOptions>
    {
        public GitHubTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => GitHubAuthenticationDefaults.AuthenticationScheme;

        public override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddGitHub(options =>
            {
                ConfigureDefaults(builder, options);
                options.Scope.Add("user:email");
            });
        }

        [Fact]
        public async Task Can_Sign_In_Using_GitHub()
        {
            // Arrange
            ConfigureTokenEndpoint();
            ConfigureUserEndpoint();
            ConfigureEmailsEndpoint();

            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaims(
                    claims,
                    (ClaimTypes.Name, "my-id"),
                    ("urn:github:name", "John Smith"),
                    (ClaimTypes.Email, "john@john-smith.local"));
            }
        }

        private void ConfigureTokenEndpoint()
            => ConfigureTokenEndpoint("https://github.com/login/oauth/access_token");

        private void ConfigureUserEndpoint()
        {
            // See https://developer.github.com/v3/users/#get-the-authenticated-user
            ConfigureUserEndpoint(
                "https://api.github.com/user",
                new
                {
                    login = "my-id",
                    name = "John Smith",
                });
        }

        private void ConfigureEmailsEndpoint()
        {
            var builder = new HttpRequestInterceptionBuilder()
                .Requests().ForGet().ForUrl("https://api.github.com/user/emails")
                .Responds().WithJsonContent(
                    new[]
                    {
                        new
                        {
                            email = "john@john-smith.local",
                            primary = true,
                        }
                    });

            Interceptor.Register(builder);
        }
    }
}
