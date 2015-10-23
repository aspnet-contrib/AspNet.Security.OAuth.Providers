/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.Fitbit
{
    /// <summary>
    /// Default values used by the Fitbit authentication middleware.
    /// </summary>
    public static class FitbitAuthenticationDefaults {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Fitbit";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.DisplayName"/>.
        /// </summary>
        public const string Displayname = "Fitbit";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Fitbit";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-Fitbit";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://www.fitbit.com/oauth2/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://api.fitbit.com/oauth2/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.fitbit.com/1/user/-/profile.json";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.Scope"/>  Modify the Scope list
        /// to customize the permissions being requested.  Defaults to: "profile".
        /// Possible options are: "activity", "nutrition", "profile", "settings", "sleep", "social", "weight".
        /// </summary>
        public static IReadOnlyList<string> Scope => new List<string> {"profile"}.AsReadOnly(); 
    }
}
