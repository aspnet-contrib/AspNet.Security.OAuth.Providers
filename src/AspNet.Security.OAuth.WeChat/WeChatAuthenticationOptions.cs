/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.WeChat.WeChatAuthenticationConstants;

namespace AspNet.Security.OAuth.WeChat
{
    /// <summary>
    /// Defines a set of options used by <see cref="WeChatAuthenticationHandler"/>.
    /// </summary>
    public class WeChatAuthenticationOptions : OAuthOptions
    {
        public WeChatAuthenticationOptions()
        {
            ClaimsIssuer = WeChatAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeChatAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeChatAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeChatAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WeChatAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("snsapi_userinfo");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(Claims.OpenId, "openid");
            ClaimActions.MapJsonKey(Claims.UnionId, "unionid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.Province, "province");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
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

        /// <summary>
        /// Use UnionID as NameIdentifier.
        /// </summary>
        /// <remarks>
        /// Enable it only when you have your WeChat MP account linked to a WeChat Open Platform account.
        /// </remarks>
        public void UseUnionId()
        {
            if (Scope.All(s => s != "snsapi_userinfo"))
            {
                throw new InvalidOperationException("Invalid scope");
            }

            ClaimActions.Remove(ClaimTypes.NameIdentifier);
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
        }
    }
}
