/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.LinkedIn
{
    public class LinkedInTests : OAuthTests<LinkedInAuthenticationOptions>
    {
        private Action<LinkedInAuthenticationOptions>? additionalConfiguration = null;

        public LinkedInTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => LinkedInAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddLinkedIn(options =>
            {
                ConfigureDefaults(builder, options);
                additionalConfiguration?.Invoke(options);
            });
        }

        protected internal override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("fr-FR"),
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1R2RtA")]
        [InlineData(ClaimTypes.Name, "Frodo Baggins")]
        [InlineData(ClaimTypes.Email, "frodo@shire.middleearth")]
        [InlineData(ClaimTypes.GivenName, "Frodo")]
        [InlineData(ClaimTypes.Surname, "Baggins")]
        [InlineData(LinkedInAuthenticationConstants.Claims.PictureUrl, "https://upload.wikimedia.org/wikipedia/en/4/4e/Elijah_Wood_as_Frodo_Baggins.png")]
        public async Task Can_Sign_In_Using_LinkedIn(string claimType, string claimValue)
        {
            // Arrange
            additionalConfiguration = options => options.Fields.Add(LinkedInAuthenticationConstants.ProfileFields.PictureUrl);

            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1R2RtA")]
        [InlineData(ClaimTypes.Name, "Frodon Sacquet")]
        [InlineData(ClaimTypes.GivenName, "Frodon")]
        [InlineData(ClaimTypes.Surname, "Sacquet")]
        public async Task Can_Sign_In_Using_LinkedIn_Localized(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "1R2RtA")]
        [InlineData(ClaimTypes.Name, "Frodon Sacquet")]
        [InlineData(ClaimTypes.GivenName, "Frodon")]
        [InlineData(ClaimTypes.Surname, "Sacquet")]
        public async Task Can_Sign_In_Using_LinkedIn_Localized_With_Custom_Resolver(string claimType, string claimValue)
        {
            // Arrange
            additionalConfiguration = options => options.MultiLocaleStringResolver = (values, preferredLocale) =>
            {
                if (values.TryGetValue("fr_FR", out string? value))
                {
                    return value;
                }

                return values.Values.FirstOrDefault();
            };

            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
