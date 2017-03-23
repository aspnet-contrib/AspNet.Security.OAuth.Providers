/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Trello {
    /// <summary>
    /// Default values used by the Trello authentication middleware.
    /// </summary>
    public static class TrelloAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Trello";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Trello";

        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Trello";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-trello";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.trello.com/1/members/me";

        /// <summary>
        /// Default value for <see cref="TrelloAuthenticationOptions.RequestTokenEndpoint"/>.
        /// </summary>
        public const string RequestTokenEndpoint = "https://trello.com/1/OAuthGetRequestToken";

        /// <summary>
        /// Default value for <see cref="TrelloAuthenticationOptions.AuthorizeTokenEndpoint"/>.
        /// </summary>
        public const string AuthorizeTokenEndpoint = "https://trello.com/1/OAuthAuthorizeToken";

        /// <summary>
        /// Default value for <see cref="TrelloAuthenticationOptions.AccessTokenEndpoint"/>.
        /// </summary>
        public const string AccessTokenEndpoint = "https://trello.com/1/OAuthGetAccessToken";
    }
}
