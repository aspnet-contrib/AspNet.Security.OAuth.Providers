/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Baidu
{
    /// <summary>
    /// Defines a set of options used by <see cref="BaiduAuthenticationHandler"/>.
    /// </summary>
    public class BaiduAuthenticationOptions : OAuthOptions
    {
        public BaiduAuthenticationOptions()
        {
            ClaimsIssuer = BaiduAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(BaiduAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BaiduAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BaiduAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BaiduAuthenticationDefaults.UserInformationEndpoint;
            
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "uname");
            ClaimActions.MapCustomJson(BaiduAuthenticationConstants.Claims.Portrait,
                user =>
                {
                    var portrait = user.Value<string>("portrait");
                    return string.IsNullOrWhiteSpace(portrait)
                        ? null
                        : $"https://tb.himg.baidu.com/sys/portrait/item/{WebUtility.UrlEncode(portrait)}";
                });
        }
    }
}
