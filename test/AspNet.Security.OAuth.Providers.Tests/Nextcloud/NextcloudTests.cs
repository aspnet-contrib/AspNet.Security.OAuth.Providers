/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Nextcloud
{
    public class NextcloudTests : OAuthTests<NextcloudAuthenticationOptions>
    {
        public NextcloudTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => NextcloudAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddNextcloud(options =>
            {
                options.AuthorizationEndpoint = "https://nextcloud.local/apps/oauth2/authorize";
                options.TokenEndpoint = "https://nextcloud.local/apps/oauth2/api/v1/token";
                options.UserInformationEndpoint = "https://nextcloud.local/ocs/v1.php/cloud/users";
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "username")]
        [InlineData(ClaimTypes.Email, "username@domain.tld")]
        [InlineData("urn:nextcloud:groups", "Group 1,Group 2,Group 3")]
        [InlineData("urn:nextcloud:username", "username")]
        [InlineData("urn:nextcloud:displayname", "Username")]
        [InlineData("urn:nextcloud:enabled", "True")]
        [InlineData("urn:nextcloud:language", "de")]
        [InlineData("urn:nextcloud:locale", "de_DE")]
        public async Task Can_Sign_In_Using_Nextcloud(string claimType, string claimValue)
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
