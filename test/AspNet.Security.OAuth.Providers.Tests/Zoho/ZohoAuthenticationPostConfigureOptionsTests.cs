/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zoho;

public static class ZohoAuthenticationPostConfigureOptionsTests
{
    [Theory]
    [InlineData(ZohoAuthenticationRegion.Australia, "accounts.zoho.com.au")]
    [InlineData(ZohoAuthenticationRegion.Canada, "accounts.zohocloud.ca")]
    [InlineData(ZohoAuthenticationRegion.Europe, "accounts.zoho.eu")]
    [InlineData(ZohoAuthenticationRegion.Global, "accounts.zoho.com")]
    [InlineData(ZohoAuthenticationRegion.India, "accounts.zoho.in")]
    [InlineData(ZohoAuthenticationRegion.Japan, "accounts.zoho.jp")]
    [InlineData(ZohoAuthenticationRegion.SaudiArabia, "accounts.zoho.sa")]
    public static void PostConfigure_Configures_Valid_Authentication_Region(ZohoAuthenticationRegion region, string domain)
    {
        // Arrange
        const string name = "Zoho";
        var target = new ZohoAuthenticationPostConfigureOptions();

        var options = new ZohoAuthenticationOptions
        {
            Region = region
        };

        // Act
        target.PostConfigure(name, options);

        // Assert
        options.AuthorizationEndpoint.ShouldBeEquivalentTo(
            $"https://{domain}{ZohoAuthenticationDefaults.AuthorizationPath}");
        Uri.TryCreate(options.AuthorizationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.TokenEndpoint.ShouldBeEquivalentTo(
            $"https://{domain}{ZohoAuthenticationDefaults.TokenPath}");
        Uri.TryCreate(options.TokenEndpoint, UriKind.Absolute, out _).ShouldBeTrue();

        options.UserInformationEndpoint.ShouldBeEquivalentTo(
            $"https://{domain}{ZohoAuthenticationDefaults.UserInformationPath}");
        Uri.TryCreate(options.UserInformationEndpoint, UriKind.Absolute, out _).ShouldBeTrue();
    }

    [Fact]
    public static void PostConfigure_Invalid_Authentication_Region_ThrowsException()
    {
        // Arrange
        const string name = "Zoho";
        var target = new ZohoAuthenticationPostConfigureOptions();

        var options = new ZohoAuthenticationOptions
        {
            Region = (ZohoAuthenticationRegion)10
        };

        // Act
        Action act = () => target.PostConfigure(name, options);
        act.ShouldThrow<InvalidOperationException>();
    }
}
