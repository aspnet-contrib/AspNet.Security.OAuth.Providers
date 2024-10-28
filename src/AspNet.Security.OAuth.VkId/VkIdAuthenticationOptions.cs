/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.VkId;

/// <summary>
/// Defines a set of options used by <see cref="VkIdAuthenticationHandler"/>.
/// </summary>
public sealed class VkIdAuthenticationOptions : OAuthOptions
{
    public VkIdAuthenticationOptions()
    {
        ClaimsIssuer = VkIdAuthenticationDefaults.ClaimsIssuer;
        CallbackPath = VkIdAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = VkIdAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = VkIdAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = VkIdAuthenticationDefaults.UserInformationEndpoint;

        // It's mandatory to use PKCE
        UsePkce = true;

        Scope.Add(VkIdAuthenticationScopes.PersonalInfo);

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
        ClaimActions.MapJsonKey(VkIdAuthenticationConstants.Claims.Avatar, "avatar");
        ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
        ClaimActions.MapJsonKey(VkIdAuthenticationConstants.Claims.IsVerified, "verified");
        ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
    }
}
