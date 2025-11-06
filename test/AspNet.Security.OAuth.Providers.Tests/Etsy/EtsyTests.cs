/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Etsy;

namespace AspNet.Security.OAuth.Providers.Tests.Etsy;

public class EtsyTests : OAuthTests<EtsyAuthenticationOptions>
{
    public EtsyTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    public override string DefaultScheme => EtsyAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddEtsy(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "789012")]
    [InlineData(ClaimTypes.Email, "test@example.com")]
    [InlineData(ClaimTypes.GivenName, "Test")]
    [InlineData(ClaimTypes.Surname, "User")]
    [InlineData("urn:etsy:user_id", "123456")]
    [InlineData("urn:etsy:shop_id", "789012")]
    [InlineData("urn:etsy:primary_email", "test@example.com")]
    [InlineData("urn:etsy:first_name", "Test")]
    [InlineData("urn:etsy:last_name", "User")]
    [InlineData("urn:etsy:image_url", "https://i.etsystatic.com/test/test_75x75.jpg")]
    public async Task Can_Sign_In_Using_Etsy(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task Does_Not_Include_Detailed_Claims_When_IncludeDetailedUserInfo_Is_False()
    {
        // Arrange: disable detailed user info enrichment
        void ConfigureServices(IServiceCollection services) => services.PostConfigureAll<EtsyAuthenticationOptions>(o => o.IncludeDetailedUserInfo = false);

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert basic claims are present
        claims.ShouldContainKey("urn:etsy:user_id");
        claims.ShouldContainKey("urn:etsy:shop_id");

        // Detailed claims should be absent when flag is false
        claims.Keys.ShouldNotContain(ClaimTypes.Email);
        claims.Keys.ShouldNotContain(ClaimTypes.GivenName);
        claims.Keys.ShouldNotContain(ClaimTypes.Surname);
        claims.Keys.ShouldNotContain("urn:etsy:primary_email");
        claims.Keys.ShouldNotContain("urn:etsy:first_name");
        claims.Keys.ShouldNotContain("urn:etsy:last_name");
        claims.Keys.ShouldNotContain("urn:etsy:image_url");
    }

    [Fact]
    public async Task Includes_Detailed_Claims_When_IncludeDetailedUserInfo_Is_True()
    {
        // Arrange: explicitly enable detailed user info enrichment (default may already be true, set explicitly for clarity)
        void ConfigureServices(IServiceCollection services) => services.PostConfigureAll<EtsyAuthenticationOptions>(o => o.IncludeDetailedUserInfo = true);

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert detailed claims are present
        claims.ShouldContainKey(ClaimTypes.Email);
        claims.ShouldContainKey(ClaimTypes.GivenName);
        claims.ShouldContainKey(ClaimTypes.Surname);
        claims.ShouldContainKey("urn:etsy:primary_email");
        claims.ShouldContainKey("urn:etsy:first_name");
        claims.ShouldContainKey("urn:etsy:last_name");
        claims.ShouldContainKey("urn:etsy:image_url");
    }
}
