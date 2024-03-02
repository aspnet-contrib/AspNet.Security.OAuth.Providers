/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Kook;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Zoom;

public class ZoomTests(ITestOutputHelper outputHelper) : OAuthTests<ZoomAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => ZoomAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddZoom(options =>
        {
            ConfigureDefaults(builder, options);
        });
    }

    protected internal override void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "0ECPVTrOjh")]
    [InlineData(ClaimTypes.Name, "Frodo Baggins")]
    [InlineData(ClaimTypes.Email, "frodo@shire.middleearth")]
    [InlineData(ClaimTypes.GivenName, "Frodo")]
    [InlineData(ClaimTypes.Surname, "Baggins")]
    [InlineData(ZoomAuthenticationConstants.Claims.Picture, "https://upload.wikimedia.org/wikipedia/en/4/4e/Elijah_Wood_as_Frodo_Baggins.png")]
    public async Task Can_Sign_In_Using_Zoom(string claimType, string claimValue)
    {
        await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
    }
}
