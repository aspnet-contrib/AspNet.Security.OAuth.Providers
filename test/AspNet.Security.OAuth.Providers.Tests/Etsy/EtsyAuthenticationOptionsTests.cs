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

    [Fact]
    public static void Validate_Throws_If_AuthorizationEndpoint_Is_Null()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AuthorizationEndpoint = null!,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>(nameof(options.AuthorizationEndpoint), options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_TokenEndpoint_Is_Null()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            TokenEndpoint = null!,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>(nameof(options.TokenEndpoint), options.Validate);
    }

    [Fact]
    public static void Validate_Throws_If_UserInformationEndpoint_Is_Null()
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            UserInformationEndpoint = null!,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentNullException>(nameof(options.UserInformationEndpoint), options.Validate);
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
        var ex = Assert.Throws<ArgumentException>(options.Validate);
        ex.ParamName.ShouldBe(nameof(options.CallbackPath));
    }
}
