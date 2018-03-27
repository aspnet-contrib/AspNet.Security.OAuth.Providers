/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
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
            AuthenticationScheme = BaiduAuthenticationDefaults.AuthenticationScheme;
            DisplayName = BaiduAuthenticationDefaults.DisplayName;
            ClaimsIssuer = BaiduAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(BaiduAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BaiduAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BaiduAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BaiduAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
