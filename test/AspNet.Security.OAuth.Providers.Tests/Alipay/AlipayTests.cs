/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Alipay;

public class AlipayTests(ITestOutputHelper outputHelper) : OAuthTests<AlipayAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => AlipayAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.Services.AddSingleton<TimeProvider, FixedClock>();
        builder.AddAlipay(options =>
        {
            ConfigureDefaults(builder, options);
            options.ClientSecret = "MIIEpAIBAAKCAQEA4qTrblG1QjselyiQQnMnyjJHH2h/mxCOYYmyJbxv+OWAjKr+ZnAamRegZSC7YJ2NxnUCNscoAa3wfnoss2RAKS23Zzs2Y47u9ZHiyW/HXyPF23qVQ/vg71YaiiMa/cLrGFlu2zy+PgpjID6xuUddFIfIJVXOBJvWU3RAzct4bMYYSNauaj5WQRYTeh23IbuCrktzEFzyqmt3EtPaIrOosL3VzL1zpuFoTh2+Fh+49wN8JbiOmLdZmf7dGVvNw6YLm6gkeT5i2N1x7L/91HLQNaT/MrllIOMf0VO3FRqSAcDy0DocBOew75ApAy623ngKW7RZAWs7Mah0MZ4mqwxjKQIDAQABAoIBAQDg36NcvSNsSH5Mmomv9NP48bPRvPxHXcD3lAi3GmW6/fNzHsH136r0VRXm4Pgpn4mo7DW7JhVSvUOOKiiqAYELmnmLqpuHYq1D6HCtTwPxKOxKnTD22DZRIgyJHNXODJT4ftvYGUflBKdfufTaka0QDr0OFjmoJvsbqJAX4Jdmy7QGNDWZB0yL66FrsXGc7EMU8UyCBPDVmRRfNqbEf35oJCP2fInDYrcjiAcrbydWh70gOvGi8Gs0ugPFVxJSKk0If1JV3iTwXV9dNTorupPQ62dKBZRV6ySe/gPgKK0Jt9uMfQxpT0w6A8OEU/bQyrFiGzmqjwqkEQXKd4fiqscFAoGBAPSI/23ZW+ZnFEucqumaAiqfvTgOvlwYuM9/RplKrW5D0YT2aG5ALjnntLth1X0Sw8aIXZfdm15spPkehgmVzVT3DalxDr1CfxJ4utOdzxqXG6/1ZBnO9o3QUY/ieVSScrtq2M+MYynaZ+i3iu5vMZ3i8YpUml2q8dugNrAtCl17AoGBAO1FMDFMK2Df5FnHYeR4JV+wlkeffEEuuCud0ruMztPvtdT6eKhI6DvF211Dl/VDle8SEt/3ygx8wiykhaKRhQxxEUFIuf7wNmXuXsu/HmLzBbtjyS5IdzY0opGItoS2L85tgeyx+4YS1J3SO/cPlur4dxvM+10oXp0W8LEPuDarAoGAM7FaPcB7GuOjeLBvuN4joxsNhvIm75USTFrdc75Dl1Gi0va78MKEgx0mKY5u8PeshyWAk3/3Pii9XyRCtXgDZfir3KvXr86EykTXSbDMfRSAd9vqA0KrACOPelknyOcEdfYKSyWkOM7AtINITUsYNAYrhVCJKU/fvMvLg8ahsE0CgYBhwhH7HcD5pwW0n9uLgJ0VcfJZDDLrwE4NWndK4tcMp0UpvREddPyKNBkPshvX58LMv4ubT8KlpnlyX07YDlDdMXDEjyxjB6HCGZZhKBti9XI1JQXs1dqYMNOSVtusjkvgJ2pSlXpmYTfM1qPyRTAPG9QnVity1IE3BA6jRTDHBQKBgQCMeM7hRmZYhqIb+645ihAmXjyZW5cWOG8XSouPb8UgDYvjgDswM1hChCxF0qsh0E0l9F/Fo4U1CI6Fl2UgO5eehcIlMy+tGLvvBAlyHJ1twsWBqmDopSh0b0S0a+kEzIf4NIBP+Gm+DJEzBig0qYeecQm8JJJzzcfrwWUkVeaKfA==";
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData("urn:alipay:avatar", "http://tfsimg.alipay.com/images/partner/T1uIxXXbpXXXXXXXX")]
    [InlineData("urn:alipay:province", "my-province")]
    [InlineData("urn:alipay:city", "my-city")]
    [InlineData("urn:alipay:nick_name", "my-nickname")]
    [InlineData("urn:alipay:gender", "M")]
    public async Task Can_Sign_In_Using_Alipay(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new AlipayAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        options.Scope.Add("scope-1");

        var redirectUrl = "https://my-site.local/signin-alipay";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new AlipayAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("app_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "auth_user,scope-1");

        if (usePkce)
        {
            query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
        else
        {
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Alipay_With_Sub_Code_Error()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AlipayAuthenticationOptions>((options) => options.ClientId = "error-id");
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<AuthenticationFailureException>(async () => await AuthenticateUserAsync(server));
        exception.InnerException!.Message.ShouldBe("An error (Code:40003 subCode:isv.not-online-app) occurred while retrieving user information.");
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "open-id")]
    [InlineData("urn:alipay:avatar", "http://tfsimg.alipay.com/images/partner/T1uIxXXbpXXXXXXXX")]
    [InlineData("urn:alipay:province", "my-province")]
    [InlineData("urn:alipay:city", "my-city")]
    [InlineData("urn:alipay:nick_name", "my-nickname")]
    [InlineData("urn:alipay:gender", "M")]
    public async Task Can_Sign_In_Using_Alipay_With_Use_Open_Id(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AlipayAuthenticationOptions>((options) => options.ClientId = "open-id");
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    private sealed class FixedClock : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(2019, 12, 14, 22, 22, 22, TimeSpan.Zero);
    }
}
