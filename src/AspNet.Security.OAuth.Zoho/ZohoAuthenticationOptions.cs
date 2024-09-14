/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Zoho;

/// <summary>
/// Defines a set of options used by <see cref="ZohoAuthenticationHandler"/>.
/// </summary>
public class ZohoAuthenticationOptions : OAuthOptions
{
    public ZohoAuthenticationOptions()
    {
        ClaimsIssuer = ZohoAuthenticationDefaults.Issuer;
        CallbackPath = ZohoAuthenticationDefaults.CallbackPath;
        AuthorizationEndpoint = ZohoAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = ZohoAuthenticationDefaults.TokenEndpoint;

        Scope.Add("AaaServer.profile.READ");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.GetString("ZUID"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("Display_Name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("Email"));
    }
}
