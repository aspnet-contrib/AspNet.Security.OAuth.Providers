/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.BattleNet;

public class BattleNetTests(ITestOutputHelper outputHelper) : OAuthTests<BattleNetAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => BattleNetAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddBattleNet(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "Unified")]
    public async Task Can_Sign_In_Using_BattleNet(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(BattleNetAuthenticationRegion.America)]
    [InlineData(BattleNetAuthenticationRegion.China)]
    [InlineData(BattleNetAuthenticationRegion.Europe)]
    [InlineData(BattleNetAuthenticationRegion.Korea)]
    [InlineData(BattleNetAuthenticationRegion.Taiwan)]
    [InlineData(BattleNetAuthenticationRegion.Unified)]
    public async Task Can_Sign_In_Using_BattleNet_Region(BattleNetAuthenticationRegion region)
    {
        // Arrange
        void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<BattleNetAuthenticationOptions>(BattleNetAuthenticationDefaults.AuthenticationScheme)
                    .Configure((options) => options.Region = region);
        }

        await AuthenticateUserAndAssertClaimValue(ClaimTypes.Name, region.ToString(), ConfigureServices);
    }

    [Fact]
    public async Task Can_Sign_In_Using_Custom_BattleNet_Region()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<BattleNetAuthenticationOptions>(BattleNetAuthenticationDefaults.AuthenticationScheme)
                    .Configure((options) =>
                    {
                        options.Region = BattleNetAuthenticationRegion.Custom;
                        options.AuthorizationEndpoint = "https://oauth.battle.local/oauth/authorize";
                        options.TokenEndpoint = "https://oauth.battle.local/oauth/token";
                        options.UserInformationEndpoint = "https://oauth.battle.local/oauth/userinfo";
                    });
        }

        await AuthenticateUserAndAssertClaimValue(ClaimTypes.Name, "Custom", ConfigureServices);
    }
}
