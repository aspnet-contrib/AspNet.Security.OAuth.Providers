/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
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
            builder.Services.AddSingleton<ISystemClock, FixedClock>();
            builder.AddDingTalk(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "dingsdsqwlklklxxxx")]
        [InlineData(ClaimTypes.Name, "杨xxx")]
        [InlineData(ClaimTypes.Email, "1@example.com")]
        [InlineData(ClaimTypes.MobilePhone, "188xxxx1234")]
        [InlineData("urn:dingtalk:avatar", "https://work.dingtalk.local/avatar.png")]
        [InlineData("urn:dingtalk:nick", "my-nick")]
        [InlineData("urn:dingtalk:active", "True")]
        [InlineData("urn:dingtalk:openid", "dingsdsqwlklklxxxx")]
        [InlineData("urn:dingtalk:unionid", "dingdkjjojoixxxx")]
        [InlineData("urn:dingtalk:main_org_auth_high_level", "True")]
        public async Task Can_Sign_In_Using_DingTalk(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        private sealed class FixedClock : ISystemClock
        {
            public DateTimeOffset UtcNow => new DateTimeOffset(2019, 12, 14, 22, 22, 22, TimeSpan.Zero);
        }
    }
}
