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

namespace AspNet.Security.OAuth.Twitch
{
    public class TwitchTests : OAuthTests<TwitchAuthenticationOptions>
    {
        public TwitchTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => TwitchAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddTwitch(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "44322889")]
        [InlineData(ClaimTypes.Name, "dallas")]
        [InlineData(ClaimTypes.Email, "login@provider.com")]
        [InlineData("urn:twitch:broadcastertype", "Broadcaster")]
        [InlineData("urn:twitch:description", "Just a gamer playing games and chatting. :)")]
        [InlineData("urn:twitch:displayname", "Dallas")]
        [InlineData("urn:twitch:offlineimageurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-channel_offline_image-1a2c906ee2c35f12-1920x1080.png")]
        [InlineData("urn:twitch:profileimageurl", "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-profile_image-1a2c906ee2c35f12-300x300.png")]
        [InlineData("urn:twitch:type", "staff")]
        public async Task Can_Sign_In_Using_Twitch(string claimType, string claimValue)
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
