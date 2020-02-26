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

namespace AspNet.Security.OAuth.Strava
{
    public class StravaTests : OAuthTests<StravaAuthenticationOptions>
    {
        public StravaTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => StravaAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddStrava(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.Country, "US")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(ClaimTypes.Gender, "Male")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.StateOrProvince, "Washington")]
        [InlineData(ClaimTypes.Surname, "Smith")]
        [InlineData("urn:strava:city", "Seattle")]
        [InlineData("urn:strava:created-at", "2019-03-17T16:12:00+00:00")]
        [InlineData("urn:strava:premium", "False")]
        [InlineData("urn:strava:profile", "https://strava.local/images/JohnSmith.png")]
        [InlineData("urn:strava:profile-medium", "https://strava.local/images/JohnSmith-medium.png")]
        [InlineData("urn:strava:updated-at", "2019-03-17T16:13:00+00:00")]
        public async Task Can_Sign_In_Using_Strava(string claimType, string claimValue)
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
