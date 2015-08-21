/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.GitHub {
    /// <summary>
    /// Default values used by the GitHub authentication middleware.
    /// </summary>
    public static class GitHubAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "GitHub";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.Caption"/>.
        /// </summary>
        public const string Caption = "GitHub";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "GitHub";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-github";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://github.com/login/oauth/access_token";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.github.com/user";
    }
}
