/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AspNet.Security.OAuth.Gitter
{
    public class GitterAuthenticationOptions : OAuthOptions
    {
        public GitterAuthenticationOptions()
        {
            ClaimsIssuer = GitterAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitterAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitterAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitterAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitterAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
        }
    }
}