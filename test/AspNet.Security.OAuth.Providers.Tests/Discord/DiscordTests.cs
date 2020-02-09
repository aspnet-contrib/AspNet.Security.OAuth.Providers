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
using static AspNet.Security.OAuth.Discord.DiscordAuthenticationConstants;

namespace AspNet.Security.OAuth.Discord
{
    public class DiscordTests : OAuthTests<DiscordAuthenticationOptions>
    {
        public DiscordTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => DiscordAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddDiscord(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(Claims.Discriminator, "1234")]
        [InlineData(Claims.AvatarHash, "dummy-avatar-hash")]
        [InlineData(Claims.AvatarUrl, "https://cdn.discordapp.com/avatars/my-id/dummy-avatar-hash.png")]
        public async Task Can_Sign_In_Using_Discord(string claimType, string claimValue)
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
