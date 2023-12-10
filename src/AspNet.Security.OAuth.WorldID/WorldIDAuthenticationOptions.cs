/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.WorldID;

/// <summary>
/// Defines a set of options used by <see cref="WorldIDAuthenticationHandler"/>.
/// </summary>
public class WorldIDAuthenticationOptions : OAuthOptions
{
    public WorldIDAuthenticationOptions()
    {
        ClaimsIssuer = WorldIDAuthenticationDefaults.Issuer;
        CallbackPath = WorldIDAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = WorldIDAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = WorldIDAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = WorldIDAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("openid");
        Scope.Add("email");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    }
}
