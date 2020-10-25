﻿/*
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
using static AspNet.Security.OAuth.KakaoTalk.KakaoTalkAuthenticationConstants;

namespace AspNet.Security.OAuth.KakaoTalk
{
    public class KakaoTalkTests : OAuthTests<KakaoTalkAuthenticationOptions>
    {
        public KakaoTalkTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => KakaoTalkAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddKakaoTalk(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "123094871203")]
        [InlineData(ClaimTypes.Name, "wplong11")]
        [InlineData(ClaimTypes.Email, "example@kakao.com")]
        [InlineData(ClaimTypes.DateOfBirth, "0103")]
        [InlineData(ClaimTypes.Gender, "male")]
        [InlineData(ClaimTypes.MobilePhone, "+82 10-1234-1234")]
        [InlineData(Claims.AgeRange, "20~29")]
        [InlineData(Claims.YearOfBirth, "2020")]
        public async Task Can_Sign_In_Using_KakaoTalk(string claimType, string claimValue)
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
