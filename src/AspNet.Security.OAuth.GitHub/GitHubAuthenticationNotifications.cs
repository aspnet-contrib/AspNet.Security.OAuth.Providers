using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.OAuth;

namespace AspNet.Security.OAuth.GitHub {
    public class GitHubAuthenticationNotifications : OAuthAuthenticationNotifications {
        public Func<GitHubAuthenticatedNotification, Task> Authenticated { get; set; } = notification => Task.FromResult<object>(null);
    }
}
