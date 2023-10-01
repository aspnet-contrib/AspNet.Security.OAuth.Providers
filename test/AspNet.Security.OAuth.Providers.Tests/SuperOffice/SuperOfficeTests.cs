/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice;

public class SuperOfficeTests(ITestOutputHelper outputHelper) : OAuthTests<SuperOfficeAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => SuperOfficeAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

        builder.AddSuperOffice(options =>
        {
            ConfigureDefaults(builder, options);

            options.ClientId = "gg454918d75b1b53101065c16ee51123";
            options.SaveTokens = true;
            options.TokenValidationParameters.ValidAudience = options.ClientId;
            options.TokenValidationParameters.ValidIssuer = "https://sod.superoffice.com";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "johm.demo.smith@superoffice.com")]
    [InlineData(ClaimTypes.Email, "johm.demo.smith@superoffice.com")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId, "4")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId, "4")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContactId, "2")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, "Cust12345")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CountryId, "826")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.GroupId, "2")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId, "826")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.PersonId, "5")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleId, "1")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleName, "User level 0")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, "2")]
    public async Task Can_Sign_In_Using_SuperOffice(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, "Cust12345")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights, "allow-bulk-export")]
    [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, "2")]
    public async Task Can_Sign_In_Using_SuperOffice_With_FunctionRights(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<SuperOfficeAuthenticationOptions>((options) =>
            {
                options.IncludeFunctionalRightsAsClaims = true;
            });
        }

        await AuthenticateUserAndAssertClaimValue(claimType, claimValue, ConfigureServices);
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_SuperOffice_With_Invalid_Token_Audience()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<SuperOfficeAuthenticationOptions>((options) =>
            {
                options.TokenValidationParameters.ValidAudience = "not-the-right-audience";
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<AuthenticationFailureException>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
    }
}
