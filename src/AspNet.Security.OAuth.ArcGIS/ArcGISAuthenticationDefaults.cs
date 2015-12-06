/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.ArcGIS {
    /// <summary>
    /// Default values used by the ArcGIS authentication middleware.
    /// </summary>
    public static class ArcGISAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "ArcGIS";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "ArcGIS";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "ArcGIS";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-arcgis";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://www.arcgis.com/sharing/rest/oauth2/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://www.arcgis.com/sharing/rest/oauth2/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://www.arcgis.com/sharing/rest/community/self";
    }
}
