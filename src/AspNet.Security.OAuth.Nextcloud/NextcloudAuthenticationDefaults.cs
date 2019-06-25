using Microsoft.AspNetCore.Authentication;

namespace AspNet.Security.OAuth.Nextcloud
{
    /// <summary>
    /// Default values used by the Nextcloud authentication middleware.
    /// </summary>
    public static class NextcloudAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Nextcloud";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Nextcloud";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Nextcloud";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-nextcloud";
    }
}
