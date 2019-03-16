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

namespace AspNet.Security.OAuth.Amazon
{
    public class AmazonTests : OAuthTests<AmazonAuthenticationOptions>
    {
        public AmazonTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AmazonAuthenticationDefaults.AuthenticationScheme;

        public override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddAmazon(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        public async Task Can_Sign_In_Using_Amazon(string claimType, string claimValue)
        {
            // Arrange
            ConfigureTokenEndpoint();
            ConfigureUserEndpoint();

            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }

        private void ConfigureTokenEndpoint()
            => ConfigureTokenEndpoint("https://api.amazon.com/auth/o2/token");

        private void ConfigureUserEndpoint()
        {
            ConfigureUserEndpoint(
                "https://api.amazon.com/user/profile?fields=email,name,user_id",
                new
                {
                    user_id = "my-id",
                    name = "John Smith",
                    email = "john@john-smith.local",
                });
        }
    }
}
