/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Weixin
{
    /// <summary>
    /// Configuration options for <see cref="WeixinAuthenticationMiddleware"/>.
    /// </summary>
    public class WeixinAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="WeixinAuthenticationOptions"/>.
        /// </summary>
        public WeixinAuthenticationOptions()
        {
            AuthenticationScheme = WeixinAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            ClaimsIssuer = WeixinAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeixinAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeixinAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeixinAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WeixinAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("snsapi_login");
            Scope.Add("snsapi_userinfo");
        }
    }
}
