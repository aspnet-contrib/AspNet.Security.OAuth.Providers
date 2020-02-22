/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.NetEase
{
    /// <summary>
    /// Defines a set of options used by <see cref="NetEaseAuthenticationHandler"/>.
    /// </summary>
    public class NetEaseAuthenticationOptions : OAuthOptions
    {
        public NetEaseAuthenticationOptions()
        {
            ClaimsIssuer = NetEaseAuthenticationDefaults.Issuer;

            CallbackPath = NetEaseAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = NetEaseAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = NetEaseAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = NetEaseAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "username");
        }
    }
}
