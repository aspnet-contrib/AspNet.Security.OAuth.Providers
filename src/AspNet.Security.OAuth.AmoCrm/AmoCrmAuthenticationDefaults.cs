/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.AmoCrm
{
    /// <summary>
    /// Default values used by the amoCRM authentication middleware.
    /// </summary>
    public static class AmoCrmAuthenticationDefaults
    {
        /// <summary>
        /// Default value for the <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "amoCRM";

        /// <summary>
        /// Default value for the <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "amoCRM";

        /// <summary>
        /// Default value for the <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "amoCRM";

        /// <summary>
        /// Default value for the <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-amocrm";

        /// <summary>
        /// Default value for the <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://www.amocrm.ru/oauth";

        /// <summary>
        /// Default value for the <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpointFormat = "https://{0}.amocrm.ru/oauth2/access_token";

        /// <summary>
        /// Default value for the <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpointFormat = "https://{0}.amocrm.ru/v3/user";
    }
}
