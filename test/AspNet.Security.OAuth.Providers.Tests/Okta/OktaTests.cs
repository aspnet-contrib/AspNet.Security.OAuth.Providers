/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Okta;

public class OktaTests(ITestOutputHelper outputHelper) : OAuthTests<OktaAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => OktaAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddOkta(options =>
        {
            ConfigureDefaults(builder, options);
            options.Domain = "okta.local";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "john.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Name, "John Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g3")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_Okta(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(ClaimTypes.Email, "jane.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "Jane")]
    [InlineData(ClaimTypes.Name, "Jane Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g4")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_Okta_With_Custom_Authorization_Server(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OktaAuthenticationOptions>("Okta", (options) =>
            {
                options.AuthorizationServer = "custom";
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }
}
