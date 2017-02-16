using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.JetbBainsHub {
    /// <summary>
    /// Configuration options for <see cref="JetBrainsHubMiddleware"/>.
    /// </summary>
    public class JetBrainsHubOptions : OAuthOptions {
        /// <summary>
        /// Initializes a new <see cref="JetBrainsHubOptions" />.
        /// </summary>
        public JetBrainsHubOptions() {
            AuthenticationScheme = JetBrainsHubDefaults.AuthenticationScheme;
            DisplayName = JetBrainsHubDefaults.AuthenticationScheme;
            CallbackPath = new PathString("/signin-jetbrainshub");
            AuthorizationEndpoint = JetBrainsHubDefaults.AuthorizationEndpoint;
            TokenEndpoint = JetBrainsHubDefaults.TokenEndpoint;
        }

        /// <summary>
        /// access_type. Set to <see cref="JetBrainsHubAccessType.Offline"/> to request a refresh token.
        /// </summary>
        public JetBrainsHubAccessType AccessType { get; set; }
    }
}