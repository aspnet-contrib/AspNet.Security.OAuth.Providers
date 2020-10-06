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

namespace AspNet.Security.OAuth.Kloudless
{
    public class KloudlessTests : OAuthTests<KloudlessAuthenticationOptions>
    {
        public KloudlessTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => KloudlessAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddKloudless(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Service, "google_calendar")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Created, "2019-03-17T13:57:00+00:00")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Active, "True")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Admin, "False")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Api, "core")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Modified, "2020-03-17T13:57:00+00:00")]
        [InlineData(KloudlessAuthenticationConstants.Claims.ServiceName, "Google Calendar")]
        [InlineData(KloudlessAuthenticationConstants.Claims.EffectiveScope, "google_calendar:normal.calendar.default:kloudless google_calendar:normal.team.default:kloudless")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Type, "account")]
        [InlineData(KloudlessAuthenticationConstants.Claims.Enabled, "True")]
        public async Task Can_Sign_In_Using_Kloudless(string claimType, string claimValue)
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
