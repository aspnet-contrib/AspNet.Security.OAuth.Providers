/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Etsy;

public static class EtsyAuthenticationOptionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public static void Validate_Does_Not_Throw_If_ClientSecret_Is_Not_Provided(string? clientSecret)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = clientSecret!,
        };

        // Act (no Assert)
        options.Validate();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public static void Validate_Throws_If_SaveTokens_Or_Pkce_Is_Disabled(bool usePkce, bool saveTokens)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            SaveTokens = saveTokens,
            UsePkce = usePkce,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>(options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_Scope_Is_Empty()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            Scope = { },
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_Scope_Does_Not_Contain_Scope_shop_r()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };
        options.Scope.Clear();
        options.Scope.Add(ClaimTypes.Email);

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_IncludeDetailedUserInfo_Is_True_But_Does_Not_Contain_Scope_email_r()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = true,
        };

        // Not Adding email scope, shop scope is already added by default

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public static void Validate_Does_Not_Throw_When_IncludeDetailedUserInfo_Is_False_And_Contains_Scope_email_r()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = false,
        };

        // Adding email scope
        options.Scope.Add(ClaimTypes.Email);

        // Act (no Assert)
        options.Validate();
    }

    [Fact]
    public static void Validate_Throws_If_CallbackPath_Is_Null()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            CallbackPath = null,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>(nameof(EtsyAuthenticationOptions.CallbackPath), options.Validate);
    }
}
