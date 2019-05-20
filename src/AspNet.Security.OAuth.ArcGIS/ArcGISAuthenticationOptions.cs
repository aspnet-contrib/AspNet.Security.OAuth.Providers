/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.ArcGIS
{
    /// <summary>
    /// Defines a set of options used by <see cref="ArcGISAuthenticationHandler"/>.
    /// </summary>
    public class ArcGISAuthenticationOptions : OAuthOptions
    {
        public ArcGISAuthenticationOptions()
        {
            ClaimsIssuer = ArcGISAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(ArcGISAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = ArcGISAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = ArcGISAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = ArcGISAuthenticationDefaults.UserInformationEndpoint;
            
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "fullName");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        }
    }
}
