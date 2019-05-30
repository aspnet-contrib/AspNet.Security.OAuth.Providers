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

namespace AspNet.Security.OAuth.MailRu
{
    public class MailRuTests : OAuthTests<MailRuAuthenticationOptions>
    {
        public MailRuTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => MailRuAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddMailRu(options =>
            {
                ConfigureDefaults(builder, options);
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "123abc")]
        [InlineData(ClaimTypes.Name, "Vasya")]
        [InlineData(ClaimTypes.Email, "vasya@mail.ru")]
        [InlineData(ClaimTypes.GivenName, "Vasiliy")]
        [InlineData(ClaimTypes.Surname, "Ivanov")]
        [InlineData("urn:mailru:profileimage", "https://filin.mail.ru/pic?d=idofpic&width=180&height=180")]
        public async Task Can_Sign_In_Using_MailRu(string claimType, string claimValue)
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
