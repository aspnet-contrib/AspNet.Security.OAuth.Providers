/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.JumpCloud;

public class JumpCloudTests(ITestOutputHelper outputHelper) : OAuthTests<JumpCloudAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => JumpCloudAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddJumpCloud(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "john.doe@example.com")]
    [InlineData(ClaimTypes.GivenName, "John")]
    [InlineData(ClaimTypes.Name, "John Doe")]
    [InlineData(ClaimTypes.NameIdentifier, "00uid4BxXw6I6TV4m0g3")]
    [InlineData(ClaimTypes.Surname, "Doe")]
    public async Task Can_Sign_In_Using_JumpCloud(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
