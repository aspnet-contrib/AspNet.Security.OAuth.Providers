/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.WeixinWebpage
{
    /// <summary>
    /// Defines a set of options used by <see cref="WeixinWebpageAuthenticationHandler"/>.
    /// </summary>
    public class WeixinWebpageAuthenticationOptions : OAuthOptions
    {
        public WeixinWebpageAuthenticationOptions()
        {
            AuthenticationScheme = WeixinWebpageAuthenticationDefaults.AuthenticationScheme;
            DisplayName = WeixinWebpageAuthenticationDefaults.DisplayName;
            ClaimsIssuer = WeixinWebpageAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeixinWebpageAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeixinWebpageAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeixinWebpageAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WeixinWebpageAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("snsapi_userinfo");
        }
    }
}
