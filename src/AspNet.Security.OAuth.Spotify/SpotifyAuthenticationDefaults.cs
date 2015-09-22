/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.Spotify {
    /// <summary>
    /// Default values used by the Spotify authentication middleware.
    /// </summary>
    public static class SpotifyAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Spotify";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.Caption"/>.
        /// </summary>
        public const string Caption = "Spotify";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Spotify";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-spotify";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://accounts.spotify.com/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://accounts.spotify.com/api/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.spotify.com/v1/me";
    }
}
