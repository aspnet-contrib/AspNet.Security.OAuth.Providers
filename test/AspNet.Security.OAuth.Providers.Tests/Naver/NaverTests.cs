/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Naver.NaverAuthenticationConstants;

namespace AspNet.Security.OAuth.Naver;

public class NaverTests(ITestOutputHelper outputHelper) : OAuthTests<NaverAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => NaverAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddNaver(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "dummywdhzWcds8IuVojuH6GoGy6WadDk")]
    [InlineData(ClaimTypes.Name, "Chris Sung")]
    [InlineData(ClaimTypes.Email, "example@naver.com")]
    [InlineData(ClaimTypes.DateOfBirth, "03-19")]
    [InlineData(ClaimTypes.Gender, "M")]
    [InlineData(ClaimTypes.MobilePhone, "010-1234-1234")]
    [InlineData(Claims.Nickname, "christallire")]
    [InlineData(Claims.Age, "30-39")]
    [InlineData(Claims.YearOfBirth, "2020")]
    [InlineData(Claims.ProfileImage, "https://ssl.pstatic.net/static/pwe/address/img_profile.png")]
    public async Task Can_Sign_In_Using_Naver(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
