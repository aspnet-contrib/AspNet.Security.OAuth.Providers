/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.Foursquare {
    /// <summary>
    /// Default values used by the Foursquare authentication middleware.
    /// </summary>
    public static class FoursquareAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Foursquare";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.Caption"/>.
        /// </summary>
        public const string Caption = "Foursquare";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Foursquare";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-foursquare";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://foursquare.com/oauth2/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://foursquare.com/oauth2/access_token";

        /// <summary>
        /// Default value for <see cref="OAuthAuthenticationOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.foursquare.com/v2/users/self";

        /// <summary>
        /// Default value for Foursquare API version.
        /// </summary>
        public const string ApiVersion = "20150927";
    }
}
