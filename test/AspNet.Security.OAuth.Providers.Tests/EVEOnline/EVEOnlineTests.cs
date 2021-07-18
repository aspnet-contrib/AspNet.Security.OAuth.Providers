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

namespace AspNet.Security.OAuth.EVEOnline
{
    public class EVEOnlineTests : OAuthTests<EVEOnlineAuthenticationOptions>
    {
        public EVEOnlineTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => EVEOnlineAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddEVEOnline(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "123123")]
        [InlineData(ClaimTypes.Name, "Some Bloke")]
        [InlineData(ClaimTypes.Expiration, "2018-08-16T09:41:44+00:00")]
        [InlineData(EVEOnlineAuthenticationConstants.Claims.Scopes, "esi-skills.read_skills.v1 esi-skills.read_skillqueue.v1")]
        public async Task Can_Sign_In_Using_EVE_Online(string claimType, string claimValue)
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
