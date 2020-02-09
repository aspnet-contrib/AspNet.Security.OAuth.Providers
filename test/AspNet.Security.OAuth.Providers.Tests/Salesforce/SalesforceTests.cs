/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Salesforce
{
    public class SalesforceTests : OAuthTests<SalesforceAuthenticationOptions>
    {
        public SalesforceTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => SalesforceAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSalesforce(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "005x0000001S2b9")]
        [InlineData(ClaimTypes.Name, "alanvan")]
        [InlineData("urn:salesforce:email", "admin@2060747062579699.com")]
        [InlineData("urn:salesforce:rest_url", "https://yourInstance.salesforce.com/services/data/v{version}/")]
        [InlineData("urn:salesforce:thumbnail_photo", "https://yourInstance.salesforce.com/profilephoto/005/T")]
        [InlineData("urn:salesforce:utc_offset", "-28800000")]
        public async Task Can_Sign_In_Using_Salesforce(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
