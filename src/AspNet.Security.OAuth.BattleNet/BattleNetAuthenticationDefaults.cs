/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.BattleNet {
    /// <summary>
    /// Default values used by the Battle.net authentication middleware.
    /// </summary>
    public static class BattleNetAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "BattleNet";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.Caption"/>.
        /// </summary>
        public const string Caption = "BattleNet";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "BattleNet";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-battlenet";

        public static class US {
            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for US servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://us.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for US servers.
            /// </summary>
            public const string TokenEndpoint = "https://us.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/> for US servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://us.api.battle.net/account/user";
        }

        public static class EU {
            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for EU servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://eu.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for EU servers.
            /// </summary>
            public const string TokenEndpoint = "https://eu.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/> for EU servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://eu.api.battle.net/account/user";
        }

        public static class KR {
            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for KR servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://kr.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for KR servers.
            /// </summary>
            public const string TokenEndpoint = "https://kr.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/> for KR servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://kr.api.battle.net/account/user";
        }

        public static class TW {
            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for TW servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://tw.battle.net/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for TW servers.
            /// </summary>
            public const string TokenEndpoint = "https://tw.battle.net/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/> for TW servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://tw.api.battle.net/account/user";
        }

        public static class CN {
            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for CN servers.
            /// </summary>
            public const string AuthorizationEndpoint = "https://www.battlenet.com.cn/oauth/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for CN servers.
            /// </summary>
            public const string TokenEndpoint = "https://www.battlenet.com.cn/oauth/token";

            /// <summary>
            /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/> for CN servers.
            /// </summary>
            public const string UserInformationEndpoint = "https://api.battlenet.com.cn/account/user";
        }
    }
}
