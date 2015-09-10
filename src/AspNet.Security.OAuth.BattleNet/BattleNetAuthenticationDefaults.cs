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

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for US servers.
		/// </summary>
		public const string AuthorizationEndpointUS = "https://us.battle.net/oauth/authorize";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for EU servers.
		/// </summary>
		public const string AuthorizationEndpointEU = "https://eu.battle.net/oauth/authorize";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for KR servers.
		/// </summary>
		public const string AuthorizationEndpointKR = "https://kr.battle.net/oauth/authorize";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for TW servers.
		/// </summary>
		public const string AuthorizationEndpointTW = "https://tw.battle.net/oauth/authorize";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/> for CN servers.
		/// </summary>
		public const string AuthorizationEndpointCN = "https://www.battlenet.com.cn/oauth/authorize";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for US servers.
		/// </summary>
		public const string TokenEndpointUS = "https://us.battle.net/oauth/token";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for EU servers.
		/// </summary>
		public const string TokenEndpointEU = "https://eu.battle.net/oauth/token";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for KR servers.
		/// </summary>
		public const string TokenEndpointKR = "https://kr.battle.net/oauth/token";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for TW servers.
		/// </summary>
		public const string TokenEndpointTW = "https://tw.battle.net/oauth/token";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/> for CN servers.
		/// </summary>
		public const string TokenEndpointCN = "https://www.battlenet.com.cn/oauth/token";

		/// <summary>
		/// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/>.
		/// </summary>
		public const string UserInformationEndpoint = "https://us.api.battle.net/account/user"; 
    }

	public enum ServerRegion
	{
		US,
		EU,
		TW,
		KR,
		CN
	}
}
