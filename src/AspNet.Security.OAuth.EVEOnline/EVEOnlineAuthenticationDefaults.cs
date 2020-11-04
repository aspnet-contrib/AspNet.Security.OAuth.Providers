/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.EVEOnline
{
    /// <summary>
    /// Default values used by the EVEOnline authentication middleware.
    /// </summary>
    public static class EVEOnlineAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-eveonline";

        /// <summary>
        /// Default values for the Tranquility (live) server.
        /// </summary>
        public static class Tranquility
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://login.eveonline.com/v2/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://login.eveonline.com/v2/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            [Obsolete("This value is no longer required for V2, information is now stored in the JWT.")]
            public const string UserInformationEndpoint = "https://login.eveonline.com/oauth/verify";
        }

        /// <summary>
        /// Default values for the Singularity (test) server.
        /// </summary>
        public static class Singularity
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://sisilogin.testeveonline.com/v2/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://sisilogin.testeveonline.com/v2/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            [Obsolete("This value is no longer required for V2, information is now stored in the JWT.")]
            public const string UserInformationEndpoint = "https://sisilogin.testeveonline.com/oauth/verify";
        }
    }
}
