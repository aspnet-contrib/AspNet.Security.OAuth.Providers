/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace AspNet.Security.OAuth.VkId;

public class VkIdTests : OAuthTests<VkIdAuthenticationOptions>
{
    public VkIdTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        LoopbackRedirectHandler.LoopbackParameters.Add("device_id", "1111");
        LoopbackRedirectHandler.LoopbackParameters.Add("type", "code_v2");
    }

    public override string DefaultScheme => VkIdAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddVkId(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1234567890")]
    [InlineData(ClaimTypes.GivenName, "Ivan")]
    [InlineData(ClaimTypes.Surname, "Ivanov")]
    [InlineData(VkIdAuthenticationConstants.Claims.Avatar, "https://pp.userapi.com/60tZWMo4SmwcploUVl9XEt8ufnTTvDUmQ6Bj1g/mmv1pcj63C4.png")]
    [InlineData(ClaimTypes.Gender, "2")]
    [InlineData(VkIdAuthenticationConstants.Claims.IsVerified, "False")]
    [InlineData(ClaimTypes.DateOfBirth, "01.01.2000")]
    public async Task Can_Sign_In_Using_VkId(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
