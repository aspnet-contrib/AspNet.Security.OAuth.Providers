/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.WordPress {
    /// <summary>
    /// Default values used by the WordPress authentication middleware.
    /// </summary>
    public class WordPressAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "WordPress";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.Caption"/>.
        /// </summary>
        public const string Caption = "WordPress";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "WordPress";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-wordpress";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://public-api.wordpress.com/oauth2/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://public-api.wordpress.com/oauth2/token";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://public-api.wordpress.com/rest/v1.1/me";
    }
}