/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Etsy;
using static AspNet.Security.OAuth.Etsy.EtsyAuthenticationConstants;

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
    [InlineData(ClaimTypes.NameIdentifier, "123456")]
    [InlineData("urn:etsy:shop_id", "789012")]
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
        claims.ShouldContainKey(ClaimTypes.NameIdentifier);
        claims.ShouldContainKey(Claims.ShopId);

        // Detailed claims should be absent when flag is false
        claims.Keys.ShouldNotContain(ClaimTypes.Email);
        claims.Keys.ShouldNotContain(ClaimTypes.GivenName);
        claims.Keys.ShouldNotContain(ClaimTypes.Surname);
        claims.Keys.ShouldNotContain(Claims.ImageUrl);
    }

    [Fact]
    public async Task Includes_Detailed_Claims_When_IncludeDetailedUserInfo_Is_True()
    {
        // Arrange: enable detailed user info, configure claims to map.
        // Note: email_r will be auto-added by the provider's post-configure step.
        void ConfigureServices(IServiceCollection services) => services.PostConfigureAll<EtsyAuthenticationOptions>(o =>
        {
            o.IncludeDetailedUserInfo = true;

            // User to include image claim
            o.ClaimActions.MapImageClaim();
        });

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert detailed claims are present
        claims.ShouldContainKey(ClaimTypes.Email);
        claims.ShouldContainKey(ClaimTypes.GivenName);
        claims.ShouldContainKey(ClaimTypes.Surname);
        claims.ShouldContainKey(Claims.ImageUrl);
    }
}
