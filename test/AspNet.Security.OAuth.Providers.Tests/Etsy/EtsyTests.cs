/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth.Providers.Tests.Etsy;

public class EtsyTests : OAuthTests<EtsyAuthenticationOptions>
{
    public EtsyTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    public override string DefaultScheme => EtsyAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddEtsy(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "789012")] // shop_id and user_id is used as primary identifier!
    [InlineData(ClaimTypes.Email, "test@example.com")]
    [InlineData(ClaimTypes.GivenName, "Test")]
    [InlineData(ClaimTypes.Surname, "User")]
    [InlineData("urn:etsy:user_id", "123456")]
    [InlineData("urn:etsy:shop_id", "789012")]
    [InlineData("urn:etsy:primary_email", "test@example.com")]
    [InlineData("urn:etsy:first_name", "Test")]
    [InlineData("urn:etsy:last_name", "User")]
    [InlineData("urn:etsy:image_url", "https://i.etsystatic.com/test/test_75x75.jpg")]
    public async Task Can_Sign_In_Using_Etsy(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
