/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.WorkWeixin
{
    public class WorkWeixinAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "WorkWeixin";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "WorkWeixin";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-workweixin";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "WorkWeixin";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://open.work.weixin.qq.com/wwopen/sso/qrConnect";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/user/get";

        /// <summary>
        /// Default value for <see cref="WorkWeixinAuthenticationOptions.UserIdentificationEndpoint"/>.
        /// </summary>
        public const string UserIdentificationEndpoint = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";
    }
}
