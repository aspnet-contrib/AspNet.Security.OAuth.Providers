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
    /// <summary>
    /// Gets or sets a value that determines whether development or production endpoints are used.
    /// The default value of this property is <see cref="ZohoAuthenticationRegion.Global"/>.
    /// </summary>
    public ZohoAuthenticationRegion Region { get; set; }

    public ZohoAuthenticationOptions()
    {
        ClaimsIssuer = ZohoAuthenticationDefaults.Issuer;
        CallbackPath = ZohoAuthenticationDefaults.CallbackPath;
        Region = ZohoAuthenticationRegion.Global;

        Scope.Add("AaaServer.profile.READ");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.GetString("ZUID"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("Display_Name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("Email"));
    }
}
