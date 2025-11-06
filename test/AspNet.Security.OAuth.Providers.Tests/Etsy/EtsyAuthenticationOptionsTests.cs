/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Etsy;

public static class EtsyAuthenticationOptionsTests
{
    public static TheoryData<EtsyAuthenticationAccessType> AccessTypes => new()
    {
        { EtsyAuthenticationAccessType.Personal }, // Private Etsy API access does not use client secret (aka 'shared secret' in Etsy App Registration) https://developers.etsy.com/documentation/essentials/authentication

        // { EtsyAuthenticationAccessType.Commercial } // TODO: Verify commercial access app registration Authentication does support confidential clients. Etsy docs do not indicate this to be used at all but support stated this would be required for token refresh
    };

    [Theory]
    [InlineData(null, EtsyAuthenticationAccessType.Personal)]
    [InlineData("", EtsyAuthenticationAccessType.Personal)]
    public static void Validate_Does_Not_Throw_If_ClientSecret_Is_Not_Provided_For_Public_Access_Type(string? clientSecret, EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = clientSecret!,
        };

        // Act (no Assert)
        options.Validate();
    }

    // Leaving this test commented out to for case that ClientSecret is used with commercial access type - so this can be reactivated
    // [Theory]
    // [InlineData(EtsyAuthenticationAccessType.Commercial)]
    // public static void Validate_Throws_If_ClientSecret_Is_Null(EtsyAuthenticationAccessType accessType)
    // {
    //    // Arrange
    //    var options = new EtsyAuthenticationOptions()
    //    {
    //        AccessType = accessType,
    //        ClientId = "my-client-id",
    //        ClientSecret = null!,
    //    };
    //
    //    // Act and Assert
    //    _ = Assert.Throws<ArgumentNullException>("ClientSecret", options.Validate);
    // }
    [Theory]
    [InlineData(EtsyAuthenticationAccessType.Personal, true, false)]
    [InlineData(EtsyAuthenticationAccessType.Personal, false, false)]
    [InlineData(EtsyAuthenticationAccessType.Personal, false, true)]
    public static void Validate_Throws_If_SaveTokens_Or_Pkce_Is_Disabled(EtsyAuthenticationAccessType accessType, bool usePkce, bool saveTokens)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            SaveTokens = saveTokens,
            UsePkce = usePkce,
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>(options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_Scope_Is_Empty(EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            Scope = { },
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_Scope_Does_Not_Contain_Scope_shop_r(EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };
        options.Scope.Clear();
        options.Scope.Add(ClaimTypes.Email);

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_IncludeDetailedUserInfo_Is_True_But_Does_Not_Contain_Scope_email_r(EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = true,
        };

        // Not Adding email scope, shop scope is already added by default

        // Act and Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Does_Not_Throw_When_IncludeDetailedUserInfo_Is_False_And_Contains_Scope_email_r(EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
            IncludeDetailedUserInfo = false,
        };

        // Adding email scope
        options.Scope.Add(ClaimTypes.Email);

        // Act (no Assert)
        options.Validate();
    }

    [Theory]
    [MemberData(nameof(AccessTypes))]
    public static void Validate_Throws_If_CallbackPath_Is_Null(EtsyAuthenticationAccessType accessType)
    {
        // Arrange
        var options = new EtsyAuthenticationOptions()
        {
            AccessType = accessType,
            CallbackPath = null,
            ClientId = "my-client-id",
            ClientSecret = "my-client-secret",
        };

        // Act and Assert
        _ = Assert.Throws<ArgumentException>(nameof(EtsyAuthenticationOptions.CallbackPath), options.Validate);
    }
}
