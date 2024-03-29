﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.BattleNet;

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
    public static readonly string DisplayName = "BattleNet";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "BattleNet";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-battlenet";

    /// <summary>
    /// A class containing the URLs for the unified global BattleNet OAuth servers.
    /// </summary>
    public static class Unified
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://oauth.battle.net/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static readonly string TokenEndpoint = "https://oauth.battle.net/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://oauth.battle.net/oauth/userinfo";
    }

    /// <summary>
    /// A class containing the URLs for the BattleNet OAuth servers for America.
    /// </summary>
    public static class America
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for US servers.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://us.battle.net/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for US servers.
        /// </summary>
        public static readonly string TokenEndpoint = "https://us.battle.net/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for US servers.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://us.battle.net/oauth/userinfo";
    }

    /// <summary>
    /// A class containing the URLs for the BattleNet OAuth servers for Europe.
    /// </summary>
    public static class Europe
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for European servers.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://eu.battle.net/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for European servers.
        /// </summary>
        public static readonly string TokenEndpoint = "https://eu.battle.net/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for European servers.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://eu.battle.net/oauth/userinfo";
    }

    /// <summary>
    /// A class containing the URLs for the BattleNet OAuth servers for Korea.
    /// </summary>
    public static class Korea
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for Korean servers.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://kr.battle.net/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for Korean servers.
        /// </summary>
        public static readonly string TokenEndpoint = "https://kr.battle.net/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for Korean servers.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://kr.battle.net/oauth/userinfo";
    }

    /// <summary>
    /// A class containing the URLs for the BattleNet OAuth servers for Taiwan.
    /// </summary>
    public static class Taiwan
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for Taiwanese servers.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://tw.battle.net/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for Taiwanese servers.
        /// </summary>
        public static readonly string TokenEndpoint = "https://tw.battle.net/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for Taiwanese servers.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://tw.battle.net/oauth/userinfo";
    }

    /// <summary>
    /// A class containing the URLs for the BattleNet OAuth servers for China.
    /// </summary>
    public static class China
    {
        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/> for China servers.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://www.battlenet.com.cn/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/> for China servers.
        /// </summary>
        public static readonly string TokenEndpoint = "https://www.battlenet.com.cn/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> for China servers.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://www.battlenet.com.cn/oauth/userinfo";
    }
}
