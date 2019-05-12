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

namespace AspNet.Security.OAuth.Paypal
{
    public class PaypalTests : OAuthTests<PaypalAuthenticationOptions>
    {
        public PaypalTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => PaypalAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddPaypal(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "mWq6_1sU85v5EG9yHdPxJRrhGHrnMJ-1PQKtX6pcsmA")]
        [InlineData(ClaimTypes.Name, "identity test")]
        [InlineData(ClaimTypes.Email, "user1@example.com")]
        [InlineData(ClaimTypes.GivenName, "identity")]
        [InlineData(ClaimTypes.Surname, "test")]
        public async Task Can_Sign_In_Using_Paypal(string claimType, string claimValue)
        {
            // Arrange
            using (var server = CreateTestServer())
            {
                // Act
                var claims = await AuthenticateUserAsync(server);

                // Assert
                AssertClaim(claims, claimType, claimValue);
            }
        }
    }
}
