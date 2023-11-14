/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Shopify;

public class ShopifyTests : OAuthTests<ShopifyAuthenticationOptions>
{
    private const string TestShopName = "apple";

    public ShopifyTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
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
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new ShopifyAuthenticationOptions()
        {
            AuthorizationEndpoint = "https://apple.myshopify.com.myshopify.com/admin/oauth/authorize",
            UsePkce = usePkce,
        };

        var properties = new AuthenticationProperties();
        properties.Items["GrantOptions"] = "per-user";
        properties.Items["ShopName"] = "Apple";

        var redirectUrl = "https://my-site.local/signin-shopify";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new ShopifyAuthenticationHandler(options, loggerFactory, encoder),
            properties);

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://apple.myshopify.com.myshopify.com/admin/oauth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("scope", "scope-1,scope-2");
        query.ShouldContainKeyAndValue("grant_options[]", "per-user");

        if (usePkce)
        {
            query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
        else
        {
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
    }
}
