/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Vimeo.VimeoAuthenticationConstants;

namespace AspNet.Security.OAuth.Vimeo
{
    /// <summary>
    /// Defines a set of options used by <see cref="VimeoAuthenticationHandler"/>.
    /// </summary>
    public class VimeoAuthenticationOptions : OAuthOptions
    {
        public VimeoAuthenticationOptions()
        {
            ClaimsIssuer = VimeoAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(VimeoAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = VimeoAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = VimeoAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = VimeoAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(Claims.FullName, "name");
            ClaimActions.MapJsonKey(Claims.ProfileUrl, "link");
            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.Value<string>("uri")?.Split('/')?.LastOrDefault());
        }
    }
}
