/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.Yahoo {
    /// <summary>
    /// Default values used by the Yahoo authentication middleware.
    /// </summary>
    public static class YahooAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Yahoo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.Caption"/>.
        /// </summary>
        public const string Caption = "Yahoo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Yahoo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-yahoo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://api.login.yahoo.com/oauth2/request_auth";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://api.login.yahoo.com/oauth2/get_token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://social.yahooapis.com/v1/user/me/profile";
    }
}