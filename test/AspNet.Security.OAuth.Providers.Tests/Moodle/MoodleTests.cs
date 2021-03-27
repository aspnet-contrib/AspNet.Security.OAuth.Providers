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
using static AspNet.Security.OAuth.Moodle.MoodleAuthenticationConstants;

namespace AspNet.Security.OAuth.Moodle
{
    public class MoodleTests : OAuthTests<MoodleAuthenticationOptions>
    {
        public MoodleTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => MoodleAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddMoodle(options =>
            {
                ConfigureDefaults(builder, options);
                options.Domain = "moodle.local";
            });
        }

        [Theory]
        [InlineData(ClaimTypes.Name, "John Doe")]
        [InlineData(ClaimTypes.NameIdentifier, "johndoe")]
        [InlineData(ClaimTypes.Email, "john.doe@example.com")]
        [InlineData(ClaimTypes.Surname, "Doe")]
        [InlineData(ClaimTypes.GivenName, "John")]
        [InlineData(ClaimTypes.MobilePhone, "7654321")]
        [InlineData(Claims.IdNumber, "1234567")]
        [InlineData(Claims.MoodleId, "22")]
        [InlineData(Claims.Language, "en")]
        public async Task Can_Sign_In_Using_Moodle(string claimType, string claimValue)
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
