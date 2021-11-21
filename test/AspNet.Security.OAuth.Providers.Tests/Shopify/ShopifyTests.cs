/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Shopify;

public class ShopifyTests : OAuthTests<ShopifyAuthenticationOptions>
{
    private const string TestShopName = "apple";

    public ShopifyTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        LoopbackRedirectHandler.LoopbackParameters.Add("shop", "apple.myshopify.com");
    }

    public override string DefaultScheme => ShopifyAuthenticationDefaults.AuthenticationScheme;

    protected internal override Task ChallengeAsync(HttpContext context) => context.ChallengeAsync(new ShopifyAuthenticationProperties(TestShopName));

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddShopify(options => ConfigureDefaults(builder, options));
    }

    protected override void ConfigureDefaults(AuthenticationBuilder builder, ShopifyAuthenticationOptions options)
    {
        base.ConfigureDefaults(builder, options);

        options.AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture, ShopifyAuthenticationDefaults.AuthorizationEndpointFormat, TestShopName);
        options.TokenEndpoint = string.Format(CultureInfo.InvariantCulture, ShopifyAuthenticationDefaults.TokenEndpointFormat, TestShopName);
        options.UserInformationEndpoint = string.Format(CultureInfo.InvariantCulture, ShopifyAuthenticationDefaults.UserInformationEndpointFormat, TestShopName);
    }

    [Theory]
    [InlineData(ClaimTypes.Country, "US")]
    [InlineData(ClaimTypes.Email, "steve@apple.com")]
    [InlineData(ClaimTypes.Name, "Apple Computers")]
    [InlineData(ClaimTypes.NameIdentifier, "apple.myshopify.com")]
    [InlineData("urn:shopify:plan_name", "enterprise")]
    public async Task Can_Sign_In_Using_Shopify(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
