/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Discord
{
    /// <summary>
    /// Defines a list of "prompt" options used to determine how the authorization flow handles existing authorizations with Discord.
    /// </summary>
    public enum DiscordAuthenticationPrompt
    {
        /// <summary>
        /// The "Omit" option is the default value, and the "prompt" query string parameter will be omitted from the Authorization Endpoint URL.
        /// </summary>
        Omit = 0,

        /// <summary>
        /// If the option of "None" is selected, the authorization screen will be skipped and redirect the user back to the redirect URI without requesting user authorization.
        /// </summary>
        None = 1,

        /// <summary>
        ///  If the option of "Consent" is selected, and if a user has previously authorized the application with the requested scopes, the user will be requested to re-approve their authorization.
        /// </summary>
        Consent = 2
    }
}
