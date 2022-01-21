/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Twitter;

/// <summary>
/// Contains constants specific to the <see cref="TwitterAuthenticationHandler"/>.
/// </summary>
public static class TwitterAuthenticationConstants
{
    /// <summary>
    /// Contains claim types specific to Twitter.
    /// </summary>
    public static class Claims
    {
        /// <summary>
        /// The claim for the friendly name of a user, as shown on their profile.
        /// </summary>
        public const string Name = "urn:twitter:name";
    }
}
