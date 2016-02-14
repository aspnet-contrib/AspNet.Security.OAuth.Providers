/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.LinkedIn {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from LinkedIn after a successful authentication process.
    /// </summary>
    public class LinkedInAuthenticationHelper {
        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("emailAddress");

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject user) => user.Value<string>("formattedName");

        /// <summary>
        /// Gets the profile picture URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfilePictureUrl([NotNull] JObject user) => user.Value<string>("pictureUrl");

        /// <summary>
        /// Gets the public profile URL corresponding to the authenticated user.
        /// </summary>
        public static string GetPublicProfileUrl([NotNull] JObject user) => user.Value<string>("publicProfileUrl");
    }
}