using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.GitHub
{
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

        public string Identifier { get; }

        public string Login { get; }

        public string Name { get; }

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
