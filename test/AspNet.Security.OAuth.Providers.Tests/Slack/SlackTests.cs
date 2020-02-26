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

namespace AspNet.Security.OAuth.Slack
{
    public class SlackTests : OAuthTests<SlackAuthenticationOptions>
    {
        public SlackTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => SlackAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddSlack(options =>
            {
                ConfigureDefaults(builder, options);
                options.Scope.Add("identity.avatar");
                options.Scope.Add("identity.email");
                options.Scope.Add("identity.team");
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "T0G9PQBBK|U0G9QF9C6")]
        [InlineData(ClaimTypes.Name, "Sonny Whether")]
        [InlineData(ClaimTypes.Email, "bobby@example.com")]
        [InlineData("urn:slack:team_id", "T0G9PQBBK")]
        [InlineData("urn:slack:team_name", "Captain Fabian's Naval Supply")]
        [InlineData("urn:slack:user_id", "U0G9QF9C6")]
        public async Task Can_Sign_In_Using_Slack(string claimType, string claimValue)
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
