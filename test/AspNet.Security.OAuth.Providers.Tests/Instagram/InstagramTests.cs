/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Instagram.InstagramAuthenticationConstants;

namespace AspNet.Security.OAuth.Instagram;

public class InstagramTests(ITestOutputHelper outputHelper) : OAuthTests<InstagramAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => InstagramAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddInstagram(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.Name, "jayposiris")]
    [InlineData(ClaimTypes.NameIdentifier, "17841405793187218")]
    public async Task Can_Sign_In_Using_Instagram(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

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

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }
}
