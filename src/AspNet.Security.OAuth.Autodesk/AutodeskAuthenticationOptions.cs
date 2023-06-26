/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Autodesk.AutodeskAuthenticationConstants;

namespace AspNet.Security.OAuth.Autodesk;

/// <summary>
/// Defines a set of options used by <see cref="AutodeskAuthenticationHandler"/>.
/// </summary>
public class AutodeskAuthenticationOptions : OAuthOptions
{
    public AutodeskAuthenticationOptions()
    {
        ClaimsIssuer = AutodeskAuthenticationDefaults.Issuer;
        CallbackPath = AutodeskAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AutodeskAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AutodeskAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AutodeskAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user-profile:read");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "preferred_username");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(Claims.EmailVerified, "email_verified");
    }
}
