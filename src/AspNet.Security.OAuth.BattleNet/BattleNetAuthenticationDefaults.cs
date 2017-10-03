/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.BattleNet
{
    /// <summary>
    /// Default values used by the Battle.net authentication middleware.
    /// </summary>
    public static class BattleNetAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "BattleNet";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "BattleNet";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "BattleNet";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-battlenet";

        public static class America
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for US servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://us.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for US servers.
            /// </summary>
            public const string TokenEndpoint = "https://us.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for US servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://us.api.battle.net/account/user";
        }

        public static class Europe
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for EU servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://eu.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for EU servers.
            /// </summary>
            public const string TokenEndpoint = "https://eu.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for EU servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://eu.api.battle.net/account/user";
        }

        public static class Korea
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for KR servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://kr.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for KR servers.
            /// </summary>
            public const string TokenEndpoint = "https://kr.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for KR servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://kr.api.battle.net/account/user";
        }

        public static class Taiwan
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for TW servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://tw.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for TW servers.
            /// </summary>
            public const string TokenEndpoint = "https://tw.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for TW servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://tw.api.battle.net/account/user";
        }

        public static class China
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for CN servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://www.battlenet.com.cn/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for CN servers.
            /// </summary>
            public const string TokenEndpoint = "https://www.battlenet.com.cn/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for CN servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://api.battlenet.com.cn/account/user";
        }
    }
}
