/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Notion
{
    public class NotionTests : OAuthTests<NotionAuthenticationOptions>
    {
        public NotionTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => NotionAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddNotion(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData("urn:notion:workspace_name", "mif")]
        [InlineData("urn:notion:workspace_icon", "icon")]
        [InlineData("urn:notion:bot_id", "mybot")]
        public async Task Can_Sign_In_Using_Notion(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
