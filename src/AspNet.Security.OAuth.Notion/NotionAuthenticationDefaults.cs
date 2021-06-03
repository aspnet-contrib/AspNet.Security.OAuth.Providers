/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Notion
{
    /// <summary>
    /// Default values used by the Notion authentication middleware.
    /// </summary>
    public static class NotionAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Notion";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Notion";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Notion";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-notion";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://api.notion.com/v1/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://api.notion.com/v1/oauth/token";
    }
}
