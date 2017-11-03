/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Default values used by the Salesforce authentication middleware.
    /// </summary>
    public static class SalesforceAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Salesforce";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Salesforce";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Salesforce";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-salesforce";

        /// <summary>
        /// Default value for the Salesforce environment (production or test)
        /// </summary>
        public const SalesforceAuthenticationEnvironment Environment = SalesforceAuthenticationEnvironment.Production;

        public static class Production
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://login.salesforce.com/services/oauth2/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://login.salesforce.com/services/oauth2/token";
        }

        public static class Test
        {
            /// <summary>
            /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
            /// </summary>
            public const string AuthorizationEndpoint = "https://test.salesforce.com/services/oauth2/authorize";

            /// <summary>
            /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
            /// </summary>
            public const string TokenEndpoint = "https://test.salesforce.com/services/oauth2/token";
        }
    }
}