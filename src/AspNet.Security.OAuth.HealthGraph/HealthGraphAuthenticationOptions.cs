/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */


using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.HealthGraph
{
    /// <summary>
    /// Defines a set of options used by <see cref="HealthGraphAuthenticationHandler"/>.
    /// </summary>
    public class HealthGraphAuthenticationOptions : OAuthOptions
    {
        public HealthGraphAuthenticationOptions()
        {
            ClaimsIssuer = HealthGraphAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(HealthGraphAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = HealthGraphAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = HealthGraphAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = HealthGraphAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userID");
        }
    }
}
