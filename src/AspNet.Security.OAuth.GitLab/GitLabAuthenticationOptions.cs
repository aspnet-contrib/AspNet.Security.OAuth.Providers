/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.GitLab {
    /// <summary>
    /// Defines a set of options used by <see cref="GitLabAuthenticationHandler"/>.
    /// </summary>
    public class GitLabAuthenticationOptions : OAuthOptions {
        public GitLabAuthenticationOptions() {
            AuthenticationScheme = GitLabAuthenticationDefaults.AuthenticationScheme;
            DisplayName = GitLabAuthenticationDefaults.DisplayName;
            ClaimsIssuer = GitLabAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitLabAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitLabAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitLabAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitLabAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
