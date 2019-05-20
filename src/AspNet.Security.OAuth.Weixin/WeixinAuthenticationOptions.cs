/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.Weixin.WeixinAuthenticationConstants;

namespace AspNet.Security.OAuth.Weixin
{
    /// <summary>
    /// Defines a set of options used by <see cref="WeixinAuthenticationHandler"/>.
    /// </summary>
    public class WeixinAuthenticationOptions : OAuthOptions
    {
        public WeixinAuthenticationOptions()
        {
            ClaimsIssuer = WeixinAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeixinAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeixinAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeixinAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WeixinAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("snsapi_login");
            Scope.Add("snsapi_userinfo");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(Claims.OpenId, "openid");
            ClaimActions.MapJsonKey(Claims.Province, "province");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.HeadImgUrl, "headimgurl");
            ClaimActions.MapCustomJson(Claims.Privilege, user =>
            {
                var value = user.Value<JArray>("privilege");
                if (value == null)
                {
                    return null;
                }

                return string.Join(",", value.ToObject<string[]>());
            });
        }
    }
}
