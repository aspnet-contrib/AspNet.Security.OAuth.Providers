/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Typeform;

/// <summary>
/// Defines a set of options used by <see cref="TypeformAuthenticationHandler"/>.
/// </summary>
public class TypeformAuthenticationOptions : OAuthOptions
{
    public TypeformAuthenticationOptions()
    {
        ClaimsIssuer = TypeformAuthenticationDefaults.Issuer;
        CallbackPath = TypeformAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = TypeformAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = TypeformAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = TypeformAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("accounts:read");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.GetString("user_id"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("alias"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("email"));
    }
}
