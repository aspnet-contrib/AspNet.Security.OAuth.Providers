using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Alipay
{
    public class AlipayTests : OAuthTests<AlipayAuthenticationOptions>
    {
        public AlipayTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => AlipayAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddAlipay(options =>
            {
                ConfigureDefaults(builder, options);
                options.ValidateSignature = false;
            });
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData("urn:alipay:avatar", "http://tfsimg.alipay.com/images/partner/T1uIxXXbpXXXXXXXX")]
        [InlineData("urn:alipay:province", "my-province")]
        [InlineData("urn:alipay:city", "my-city")]
        [InlineData("urn:alipay:nick_name", "my-nickname")]
        [InlineData("urn:alipay:is_student_certified", "T")]
        [InlineData("urn:alipay:user_type", "1")]
        [InlineData("urn:alipay:user_status", "T")]
        [InlineData("urn:alipay:is_certified", "T")]
        [InlineData("urn:alipay:gender", "F")]
        public async Task Can_Sign_In_Using_Alipay(string claimType, string claimValue)
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
