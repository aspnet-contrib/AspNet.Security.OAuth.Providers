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

namespace AspNet.Security.OAuth.DingTalk
{
    public class DingTalkTests : OAuthTests<DingTalkAuthenticationOptions>
    {
        public DingTalkTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => DingTalkAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddDingTalk(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData("urn:dingtalk:dingid", "$:LWCP_v1:$***********")]
        [InlineData("urn:dingtalk:unionid", "PcgbiiujjR7gGjgggKDjPSwiEiE")]
        [InlineData("urn:dingtalk:openid", "n6GjFgKT3FADNeJoUux3FwiEiE")]
        public async Task Can_Sign_In_Using_DingTalk(string claimType, string claimValue)
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
