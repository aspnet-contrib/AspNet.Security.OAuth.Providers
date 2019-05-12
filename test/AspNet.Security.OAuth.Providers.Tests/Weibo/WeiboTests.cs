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

namespace AspNet.Security.OAuth.Weibo
{
    public class WeiboTests : OAuthTests<WeiboAuthenticationOptions>
    {
        public WeiboTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => WeiboAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddWeibo(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.Gender, "male")]
        [InlineData("urn:weibo:avatar_hd", "Avatar HD")]
        [InlineData("urn:weibo:avatar_large", "Avatar Large")]
        [InlineData("urn:weibo:cover_image_phone", "Nokia 3310")]
        [InlineData("urn:weibo:location", "The Cloud")]
        [InlineData("urn:weibo:profile_image_url", "https://weibo.local/profile.png")]
        [InlineData("urn:weibo:screen_name", "JohnSmith")]
        public async Task Can_Sign_In_Using_Weibo(string claimType, string claimValue)
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
