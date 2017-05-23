/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.QQ
{
    /// <summary>
    /// Configuration options for <see cref="QQAuthenticationMiddleware"/>.
    /// </summary>
    public class QQAuthenticationOptions : OAuthOptions
    {
        public QQAuthenticationOptions()
        {
            AuthenticationScheme = QQAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            ClaimsIssuer = QQAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(QQAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = QQAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQAuthenticationDefaults.TokenEndpoint;
            OpenIdEndpoint = QQAuthenticationDefaults.OpenIdEndpoint;
            UserInformationEndpoint = QQAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("get_user_info");
        }

        /// <summary>
        /// The endpoint for received user openid.
        /// </summary>
        public string OpenIdEndpoint
        {
            get;
            set;
        }
    }
}
