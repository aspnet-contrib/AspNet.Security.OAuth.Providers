/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Default values used by the Salesforce authentication middleware.
    /// </summary>
    public static class SalesforceAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Salesforce";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Salesforce";

        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Salesforce";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-salesforce";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://login.salesforce.com/services/oauth2/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://login.salesforce.com/services/oauth2/token";
    }
}