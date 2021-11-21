﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.EVEOnline;

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
    public static readonly string DisplayName = "EVEOnline";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "EVEOnline";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-eveonline";

    /// <summary>
    /// Default values for the Tranquility (live) server.
    /// </summary>
    public static class Tranquility
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://login.eveonline.com/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static readonly string TokenEndpoint = "https://login.eveonline.com/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://login.eveonline.com/oauth/verify";
    }

    /// <summary>
    /// Default values for the Singularity (test) server.
    /// </summary>
    public static class Singularity
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://sisilogin.testeveonline.com/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static class Tranquility
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public static readonly string AuthorizationEndpoint = "https://login.eveonline.com/v2/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public static readonly string TokenEndpoint = "https://login.eveonline.com/v2/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            [Obsolete("This value is no longer required for V2, information is now stored in the JWT.")]
            public static readonly string UserInformationEndpoint = "https://login.eveonline.com/oauth/verify";
        }

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static class Singularity
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public static readonly string AuthorizationEndpoint = "https://sisilogin.testeveonline.com/v2/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public static readonly string TokenEndpoint = "https://sisilogin.testeveonline.com/v2/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
            /// </summary>
            [Obsolete("This value is no longer required for V2, information is now stored in the JWT.")]
            public static readonly string UserInformationEndpoint = "https://sisilogin.testeveonline.com/oauth/verify";
        }
    }
}
