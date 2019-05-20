/*
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

namespace AspNet.Security.OAuth.Bitbucket
{
    public class BitbucketTests : OAuthTests<BitbucketAuthenticationOptions>
    {
        public BitbucketTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => BitbucketAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddBitbucket(options =>
            {
                ConfigureDefaults(builder, options);
                options.Scope.Add("account");
                options.Scope.Add("email");
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "johnsmith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData("urn:bitbucket:name", "John Smith")]
        [InlineData("urn:bitbucket:url", "https://bitbucket.org")]
        public async Task Can_Sign_In_Using_Bitbucket(string claimType, string claimValue)
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
