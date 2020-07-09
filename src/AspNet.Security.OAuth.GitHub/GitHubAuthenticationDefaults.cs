/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.GitHub
{
    /// <summary>
    /// Default values used by the GitHub authentication middleware.
    /// </summary>
    public static class GitHubAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "GitHub";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "GitHub";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "GitHub";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-github";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://github.com" + AuthorizationEndpointPath;

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://github.com" + TokenEndpointPath;

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://api.github.com" + UserInformationEndpointPath;

        /// <summary>
        /// Default value for <see cref="GitHubAuthenticationOptions.UserEmailsEndpoint"/>.
        /// </summary>
        public const string UserEmailsEndpoint = "https://api.github.com" + UserEmailsEndpointPath;

        /// <summary>
        /// Default path to use for the GitHub Enterprise v3 REST API.
        /// </summary>
        public const string EnterpriseApiPath = "/api/v3";

        /// <summary>
        /// Default path to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpointPath = "/login/oauth/authorize";

        /// <summary>
        /// Default path to use for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpointPath = "/login/oauth/access_token";

        /// <summary>
        /// Default path to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpointPath = "/user";

        /// <summary>
        /// Default path to use for <see cref="GitHubAuthenticationOptions.UserEmailsEndpoint"/>.
        /// </summary>
        public const string UserEmailsEndpointPath = "/user/emails";
    }
}
