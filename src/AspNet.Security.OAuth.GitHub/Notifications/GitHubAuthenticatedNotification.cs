using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.GitHub {
    public class GitHubAuthenticatedNotification : OAuthAuthenticatedContext {
        public GitHubAuthenticatedNotification(
            [NotNull] HttpContext context,
            [NotNull] OAuthAuthenticationOptions options,
            [NotNull] JObject payload,
            [NotNull] TokenResponse tokens)
            : base(context, options, payload, tokens) {
            Identifier = TryGetValue(payload, "id");
            Login = TryGetValue(payload, "login");
            Name = TryGetValue(payload, "name");
            Link = TryGetValue(payload, "url");
        }

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public string Link { get; }

        private static string TryGetValue(JObject payload, string property) {
            JToken value;
            if (payload.TryGetValue(property, out value)) {
                return value.ToString();
            }

            return null;
        }
    }
}
