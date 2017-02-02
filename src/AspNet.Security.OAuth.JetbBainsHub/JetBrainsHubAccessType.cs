namespace AspNet.Security.OAuth.JetbBainsHub {
    /// <summary>
    /// Access type for JetBrains Hub OAuth2.
    /// </summary>
    public enum JetBrainsHubAccessType {
        /// <summary>
        /// Only allows online access.
        /// </summary>
        Online,
        /// <summary>
        /// Allows offline access and issues a refresh token the first time
        /// the application enxchages an authorization code for a user.
        /// </summary>
        Offline
    }
}