/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.Vimeo {
    /// <summary>
    /// Default values used by the Vimeo authentication middleware.
    /// </summary>
    public static class VimeoAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Vimeo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Vimeo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Vimeo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-vimeo";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://api.vimeo.com/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://api.vimeo.com/oauth/access_token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.vimeo.com/me";
    }
}
