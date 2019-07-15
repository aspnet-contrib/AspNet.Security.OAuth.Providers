/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Shopify
{
    public class ShopifyTests : OAuthTests<ShopifyAuthenticationOptions>
    {
        private const string testShopName = "apple";

        public ShopifyTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;

            LoopbackRedirectHandler = new ShopifyLoopbackRedirectHandler()
            {
                RedirectUri = RedirectUri,
                ShopName = testShopName
            };
        }

        public override string DefaultScheme => ShopifyAuthenticationDefaults.AuthenticationScheme;

        protected internal override Task ChallengeAsync(HttpContext context) => context.ChallengeAsync(new ShopifyAuthenticationProperties(testShopName));

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddShopify(options => ConfigureDefaults(builder, options));
        }

        protected override void ConfigureDefaults(AuthenticationBuilder builder, ShopifyAuthenticationOptions options)
        {
            base.ConfigureDefaults(builder, options);

            options.AuthorizationEndpoint = string.Format(ShopifyAuthenticationDefaults.AuthorizationEndpointFormat, testShopName);
            options.TokenEndpoint = string.Format(ShopifyAuthenticationDefaults.TokenEndpointFormat, testShopName);
            options.UserInformationEndpoint = string.Format(ShopifyAuthenticationDefaults.UserInformationEndpointFormat, testShopName);
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "apple.myshopify.com")]
        [InlineData(ClaimTypes.Name, "Apple Computers")]
        [InlineData(ClaimTypes.Email, "steve@apple.com")]
        [InlineData(ClaimTypes.Country, "US")]
        [InlineData(ShopifyAuthenticationDefaults.ShopifyPlanNameClaimType, "enterprise")]
        public async Task Can_Sign_In_Using_Spotify(string claimType, string claimValue)
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