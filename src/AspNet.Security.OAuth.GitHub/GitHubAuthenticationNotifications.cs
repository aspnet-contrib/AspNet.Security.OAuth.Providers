using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.GitHub {
    /// <summary>
    /// Defines a set of notifications used by <see cref="GitHubAuthenticationHandler"/>.
    /// </summary>
    public class GitHubAuthenticationNotifications : OAuthAuthenticationNotifications {
        /// <summary>
        /// Invoked when a user has been succesfully authenticated via GitHub.
        /// </summary>
        public Func<GitHubAuthenticatedNotification, Task> Authenticated { get; set; } = notification => Task.FromResult<object>(null);
    }
}
