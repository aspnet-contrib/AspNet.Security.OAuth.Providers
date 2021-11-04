/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.QuickBooks;
using static AspNet.Security.OAuth.QuickBooks.QuickBooksAuthenticationConstants;

namespace AspNet.Security.OAuth.QuickBooksTests
{
    public class QuickBooksTests : OAuthTests<QuickBooksAuthenticationOptions>
    {
        public QuickBooksTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => QuickBooksAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddQuickBooks(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "2039290222")]
        [InlineData(ClaimTypes.MobilePhone, "(314)000-0000")]
        [InlineData(ClaimTypes.Email, "john.smith@test.local")]
        [InlineData(Claims.EmailVerified, "true")]
        public async Task Can_Sign_In_Using_QuickBooks(string claimType, string claimValue)
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
