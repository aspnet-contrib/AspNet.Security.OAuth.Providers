/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.ServiceChannel.ServiceChannelAuthenticationConstants;

namespace AspNet.Security.OAuth.ServiceChannel;

public class ServiceChannelTests(ITestOutputHelper outputHelper) : OAuthTests<ServiceChannelAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => ServiceChannelAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddServiceChannel(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "4034383")]
    [InlineData(ClaimTypes.Name, "john.smith@test.local")]
    [InlineData(ClaimTypes.Email, "john.smith@test.local")]
    [InlineData(Claims.ProviderId, "2000156703")]
    [InlineData(Claims.ProviderName, "uicccf")]
    public async Task Can_Sign_In_Using_ServiceChannel(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
