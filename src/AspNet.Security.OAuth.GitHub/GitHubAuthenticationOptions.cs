using System.Security.Claims;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.GitHub {
    /// <summary>
    /// Defines a set of options used by <see cref="GitHubAuthenticationHandler"/>.
    /// </summary>
    public class GitHubAuthenticationOptions : OAuthAuthenticationOptions<GitHubAuthenticationNotifications> {
        public GitHubAuthenticationOptions() {
            AuthenticationScheme = GitHubAuthenticationDefaults.AuthenticationScheme;
            Caption = GitHubAuthenticationDefaults.Caption;
            ClaimsIssuer = GitHubAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitHubAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitHubAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitHubAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitHubAuthenticationDefaults.UserInformationEndpoint;

            Notifications = new GitHubAuthenticationNotifications();
        }

        /// <summary>
        /// Defines whether access and refresh tokens should be stored in the
        /// <see cref="ClaimsPrincipal"/> after a successful authentication.
        /// You can set this property to <see cref="false"/> to reduce
        /// the size of the final authentication cookie.
        /// </summary>
        public bool SaveTokens { get; set; } = true;
    }
}
