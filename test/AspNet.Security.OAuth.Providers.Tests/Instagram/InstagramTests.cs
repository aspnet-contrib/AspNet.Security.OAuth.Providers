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
using static AspNet.Security.OAuth.Instagram.InstagramAuthenticationConstants;

namespace AspNet.Security.OAuth.Instagram
{
    public class InstagramTests : OAuthTests<InstagramAuthenticationOptions>
    {
        public InstagramTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => InstagramAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddInstagram(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.Name, "jayposiris")]
        [InlineData(ClaimTypes.NameIdentifier, "17841405793187218")]
        public async Task Can_Sign_In_Using_Instagram(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(ClaimTypes.Name, "jayposiris")]
        [InlineData(ClaimTypes.NameIdentifier, "17841405793187218")]
        [InlineData(Claims.AccountType, "PERSONAL")]
        [InlineData(Claims.MediaCount, "42")]
        public async Task Can_Sign_In_Using_Instagram_With_Additional_Fields(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<InstagramAuthenticationOptions>((options) =>
                {
                    options.Fields.Add("account_type");
                    options.Fields.Add("media_count");
                    options.Scope.Add("user_media");
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
