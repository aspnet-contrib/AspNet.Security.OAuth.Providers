/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.EVEOnline {
    /// <summary>
    /// Default values used by the EVEOnline authentication middleware.
    /// </summary>
    public static class EVEOnlineAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-eveonline";

        /// <summary>
        /// Class with default values for Tranquility (Live) server.
        /// </summary>
        public static class Tranquility
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://login.eveonline.com/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://login.eveonline.com/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            public const string UserInformationEndpoint = "https://login.eveonline.com/oauth/verify";
        }

        /// <summary>
        /// Class with default values for Singularity (Test) server.
        /// </summary>
        public static class Singularity
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://sisilogin.testeveonline.com/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://sisilogin.testeveonline.com/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            public const string UserInformationEndpoint = "https://sisilogin.testeveonline.com/oauth/verify";
        }
    }
}
