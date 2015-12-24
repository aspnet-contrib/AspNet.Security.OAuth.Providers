/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Dropbox {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Dropbox after a successful authentication process.
    /// </summary>
    public static class DropboxAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("uid");

        /// <summary>
        /// Gets the full username of the authenticated user.
        /// </summary>
        public static string GetDisplayName([NotNull] JObject user) => user.Value<string>("display_name");

        /// <summary>
        /// Gets the email address associated with the Dropbox account.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("email");
    }
}
