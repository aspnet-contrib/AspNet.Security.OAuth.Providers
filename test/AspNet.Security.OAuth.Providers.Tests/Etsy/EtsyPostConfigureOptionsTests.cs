/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Etsy;

public static class EtsyPostConfigureOptionsTests
{
    [Fact]
    public static void PostConfigure_Adds_EmailRead_Scope_When_DetailedUserInfo_Enabled_And_Not_Contains_Scope_email_r()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = true,
        };

        // Ensure email_r not already present
        options.Scope.Remove(EtsyAuthenticationConstants.Scopes.EmailRead);

        var postConfigure = new EtsyPostConfigureOptions();

        // Act
        postConfigure.PostConfigure(EtsyAuthenticationDefaults.AuthenticationScheme, options);

        // Assert
        options.Scope.ShouldContain(EtsyAuthenticationConstants.Scopes.EmailRead);
    }

    [Fact]
    public static void PostConfigure_Does_Not_Add_EmailRead_When_DetailedUserInfo_Disabled()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = false,
        };

        var postConfigure = new EtsyPostConfigureOptions();

        // Act
        postConfigure.PostConfigure(EtsyAuthenticationDefaults.AuthenticationScheme, options);

        // Assert
        options.Scope.ShouldNotContain(EtsyAuthenticationConstants.Scopes.EmailRead);
    }

    [Fact]
    public static void PostConfigure_Does_Not_Duplicate_EmailRead_Scope()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = true,
        };

        // Add the email scope manually
        options.Scope.Add(EtsyAuthenticationConstants.Scopes.EmailRead);

        var postConfigure = new EtsyPostConfigureOptions();

        // Act
        postConfigure.PostConfigure(EtsyAuthenticationDefaults.AuthenticationScheme, options);

        // Assert (will throw if duplicate exists)
        options.Scope.ShouldBeUnique();
    }
}
