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

namespace AspNet.Security.OAuth.Vkontakte
{
    public class VkontakteTests : OAuthTests<VkontakteAuthenticationOptions>
    {
        public VkontakteTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => VkontakteAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddVkontakte(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData(ClaimTypes.Hash, "1fRE977YklVfdpwiJRZWW2+oEKo=")]
        [InlineData("urn:vkontakte:photo:link", "https://vk.local/photo.png")]
        [InlineData("urn:vkontakte:photo_thumb:link", "https://vk.local/thumbnail.png")]
        public async Task Can_Sign_In_Using_Vkontakte(string claimType, string claimValue)
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
