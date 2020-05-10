/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.SuperOffice;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.SuperOffice
{
    public class SuperOfficeTests : OAuthTests<SuperOfficeAuthenticationOptions>
    {
        public SuperOfficeTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => SuperOfficeAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSuperOffice(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId, "4")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId, "4")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContactId, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId, "826")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleName, "User level 0")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleId, "1")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CountryId, "826")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.GroupId, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.PersonId, "5")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, "Cust12345")]
        public async Task Can_Sign_In_Using_SuperOffice_With_No_Token_Validation(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, MockJwtSecurityTokenHandler>();
                services.PostConfigureAll<SuperOfficeAuthenticationOptions>((options) =>
                {
                    options.ValidateTokens = false;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId, "4")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId, "4")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContactId, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId, "826")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleName, "User level 0")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.RoleId, "1")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.CountryId, "826")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.GroupId, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, "2")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.PersonId, "5")]
        [InlineData(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, "Cust12345")]
        public async Task Can_Sign_In_Using_SuperOffice(string claimType, string claimValue)
        {
            // Arrange
            static void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<JwtSecurityTokenHandler, MockJwtSecurityTokenHandler>();
                services.PostConfigureAll<SuperOfficeAuthenticationOptions>((options) =>
                {
                    options.ClientId = "gg454918d75b1b53101065c16ee51123";
                    options.TokenValidationParameters.ValidAudience = options.ClientId;
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        private sealed class MockJwtSecurityTokenHandler : JwtSecurityTokenHandler
        {
            protected override void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
            {
                // Do not validate the lifetime as the test token has expired
            }
        }
    }
}
