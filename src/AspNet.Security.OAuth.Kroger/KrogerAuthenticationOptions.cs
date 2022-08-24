/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Kroger;

/// <summary>
/// Defines a set of options used by <see cref="KrogerAuthenticationHandler"/>.
/// </summary>
public class KrogerAuthenticationOptions : OAuthOptions
{
    public KrogerAuthenticationOptions()
    {
        ClaimsIssuer = KrogerAuthenticationDefaults.Issuer;

        CallbackPath = KrogerAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = KrogerAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = KrogerAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = KrogerAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("profile.compact");

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "data", "id");
    }
}
