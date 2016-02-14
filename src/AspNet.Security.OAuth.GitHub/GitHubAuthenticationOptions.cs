/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.GitHub {
    /// <summary>
    /// Defines a set of options used by <see cref="GitHubAuthenticationHandler"/>.
    /// </summary>
    public class GitHubAuthenticationOptions : OAuthOptions {
        public GitHubAuthenticationOptions() {
            AuthenticationScheme = GitHubAuthenticationDefaults.AuthenticationScheme;
            DisplayName = GitHubAuthenticationDefaults.DisplayName;
            ClaimsIssuer = GitHubAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitHubAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitHubAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitHubAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitHubAuthenticationDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; } = GitHubAuthenticationDefaults.UserEmailsEndpoint;
    }
}
