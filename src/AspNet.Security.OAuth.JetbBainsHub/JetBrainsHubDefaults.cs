namespace AspNet.Security.OAuth.JetbBainsHub {
    /// <summary>
    /// Default values for JetBrains Hub authentication.
    /// </summary>
    public class JetBrainsHubDefaults {
        public const string AuthenticationScheme = "JetBrains Hub";

        public const string JetBrainsHub = "https://hub.jetbrains.com";

        public static readonly string AuthorizationEndpoint = $"{JetBrainsHub}/oauth2/auth";

        public static readonly string TokenEndpoint = $"{JetBrainsHub}/oauth2/token";

        public static readonly string UserInformationEndpoint = $"{JetBrainsHub}/users/me";
    }
}