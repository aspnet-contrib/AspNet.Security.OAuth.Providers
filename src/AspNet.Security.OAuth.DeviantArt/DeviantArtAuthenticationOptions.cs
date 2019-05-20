/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.DeviantArt.DeviantArtAuthenticationConstants;

namespace AspNet.Security.OAuth.DeviantArt
{
    /// <summary>
    /// Defines a set of options used by <see cref="DeviantArtAuthenticationHandler"/>.
    /// </summary>
    public class DeviantArtAuthenticationOptions : OAuthOptions
    {
        public DeviantArtAuthenticationOptions()
        {
            ClaimsIssuer = DeviantArtAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(DeviantArtAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DeviantArtAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DeviantArtAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DeviantArtAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("user");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(Claims.Username, "username");
        }
    }
}
