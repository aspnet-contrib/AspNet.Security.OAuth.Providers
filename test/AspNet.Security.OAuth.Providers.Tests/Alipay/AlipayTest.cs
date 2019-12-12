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
                options.ClientSecret = "MIIEowIBAAKCAQEAmgyZvXctSY44AvzZsrZVezU45UP4IgdrXEn1iPz7fzy0ZYuXNYQR1K6NueQM+flB4cP9IYwdyrlDBADaP1huJr9nVKsl0rYcB3fjftyOzUfyEDU74mKcLC+6jW0gvhmI8AhgoftOaoK5//oL1LQzdka4tMeTyH865xbVP0i08zSOKxmotNfj1vC8e5LGr9UHNF7nlPNgkPXcIExVNpi7/yWB6SBCrGNqkoOjOs2dh6gad158rtvgX5EsR2K0zq2/WDLjLHw2lqieaC84n/Z4eWg7wt7wZyAwInPU8sG/v/a8amUH10ve1eznh5dkFlfzH2bZgzJ7avD1Nzgt7FDaQwIDAQABAoIBACOtS2XP1hM09s/qU1jKVZ3FQ7oFNdBbu4eSMWVagbyECJuD6q8gOSjyjCxDHqY+Df4Fb/h3BOdQZK7mq6UEC+65YWsAgC/+sRshrvRQjFoKkYsjaQ0THWw2WZQrc+vS6h39MrQRCaYtRHp+JINMgKah1mE334gNv0yJwzV210GDzHO1QytN7/UlvgOGwOIw+uohEhsbwdb++MRamtpLcjD96xxSixJlEAubUhfQxh1bBTAuCPU1I5H/J+rrZAC2YUQVb2Goy8LUqjn70ximuoXV+xZCJ1Hgr9e6vzvnuLy2wUgOkrE/HS57wqjkkBSVO5bQ1j5H9NLouOUZ5WNKOfECgYEAyNh4ATFsWpsx9lBFoxgFWrwhM5R8OLODbqs8qAJLfC6DNVgaCxeQGmg3RZ5jxQBLdpSCWSCIT/eKPvu0CiGLdny7+t3PotwLuBfXcy+dAdI4ubg3A2vGRDUk0U9ZkuUuaM7DvXQOmLva8T98blX4vOZwEL1wmchOxgk8yMxYHCcCgYEAxFpT0UPqc8LImsReb+o5LAeBZsAJpG2s/fsgfS/gsCwduf4OJ5N5TlUVHbOSzISCWDPmVz9sWEFj9s6co0DYLGmaid6+s5RI9DwRhVaL8Sl0PYaam7frNuiTedyRfMLF7n8PhHqensJQjR/xP+CZOs80KRhdvjwXv0pFS7zWNoUCgYACIUGMibCjeVfbS9ihNSUBZFNZz65Nj6HKL6iyA2w3gerESw3jpjlR/l7vrxFRyoICXOrQ9SZc6rwdlN/A58Ap3oLD00xbZsf9CMuxHgUlOsx9M7XppF/y4zljutqxUxrd46txu+RXvE5DFrBEH0dHAY6Yrtmd1+D1+q0ZWAlrKwKBgHCpLzx8DnLbSUTb9R+bsbAcole1ShMJRt/3jj2mEfKjbW8BYVe92zVhxhrjpRAp26wGI1zeLCk7Y8MB64gUNbTN5vjCUIMzSuSv7pGmBeealHKU3/MHBTPdIHkkYGnIS887IkkuHgMTlSpUMJUpJmJC7zfBHlB/pFSCpd39/J8NAoGBAJn+/16vao78AuXk62OlmsNf9yGMDQpAH+vswjO+HwwG5845D3T4fNH1qfo5jc592/8LpEtuTDkMwHRz34MPTIwVJpkQrHhoW74YlQMU5rTM23gETVXtX91rUZS71rd9EdBB34vexLqHNyD+1/cH5MRdgW9ZpM34IsaGuvXKaS/e";
                options.NotValidateSign = true;
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
