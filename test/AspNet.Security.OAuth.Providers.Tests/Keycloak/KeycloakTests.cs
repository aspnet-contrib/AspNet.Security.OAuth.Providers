/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Keycloak;

public class KeycloakTests(ITestOutputHelper outputHelper) : OAuthTests<KeycloakAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => KeycloakAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddKeycloak(options =>
        {
            ConfigureDefaults(builder, options);
            options.Domain = "keycloak.local";
            options.Realm = "myrealm";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData(ClaimTypes.Email, "john@smith.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Role, "admin")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    public async Task Can_Sign_In_Using_Keycloak_BaseAddress(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<KeycloakAuthenticationOptions>((options) =>
            {
                options.BaseAddress = new Uri("http://keycloak.local:8080");
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }

    [Theory]
    [InlineData(null, ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData(null, ClaimTypes.Email, "john@smith.com")]
    [InlineData(null, ClaimTypes.GivenName, "John")]
    [InlineData(null, ClaimTypes.Role, "admin")]
    [InlineData(null, ClaimTypes.Name, "John Smith")]
    [InlineData("17.0", ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData("17.0", ClaimTypes.Email, "john@smith.com")]
    [InlineData("17.0", ClaimTypes.GivenName, "John")]
    [InlineData("17.0", ClaimTypes.Role, "admin")]
    [InlineData("17.0", ClaimTypes.Name, "John Smith")]
    [InlineData("18.0", ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData("18.0", ClaimTypes.Email, "john@smith.com")]
    [InlineData("18.0", ClaimTypes.GivenName, "John")]
    [InlineData("18.0", ClaimTypes.Role, "admin")]
    [InlineData("18.0", ClaimTypes.Name, "John Smith")]
    [InlineData("19.0", ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData("19.0", ClaimTypes.Email, "john@smith.com")]
    [InlineData("19.0", ClaimTypes.GivenName, "John")]
    [InlineData("19.0", ClaimTypes.Role, "admin")]
    [InlineData("19.0", ClaimTypes.Name, "John Smith")]
    public async Task Can_Sign_In_Using_Keycloak_Domain(string? version, string claimType, string claimValue)
    {
        // Arrange
        void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<KeycloakAuthenticationOptions>((options) =>
            {
                options.Domain = "keycloak.local";

                if (version is not null)
                {
                    options.Version = Version.Parse(version);
                }
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "995c1500-0dca-495e-ba72-2499d370d181")]
    [InlineData(ClaimTypes.Email, "john@smith.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Role, "admin")]
    [InlineData(ClaimTypes.Name, "John Smith")]
    public async Task Can_Sign_In_Using_Keycloak_Public_AccessType(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<KeycloakAuthenticationOptions>((options) =>
            {
                options.AccessType = KeycloakAuthenticationAccessType.Public;
                options.ClientSecret = string.Empty;
                options.Domain = "keycloak.local";
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }
}
