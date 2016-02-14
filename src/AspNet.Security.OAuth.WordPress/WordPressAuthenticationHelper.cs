/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.WordPress {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from WordPress after a successful authentication process.
    /// </summary>
    public static class WordPressAuthenticationHelper {
        /// <summary>
        /// Gets the avatar URL corresponding to the authenticated user.
        /// </summary>
        public static string GetAvatarUrl([NotNull] JObject user) => user.Value<string>("avatar_URL");

        /// <summary>
        /// Gets the display name corresponding to the authenticated user.
        /// </summary>
        public static string GetDisplayName([NotNull] JObject user) => user.Value<string>("display_name");

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("email");

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("ID");

        /// <summary>
        /// Gets the primary blog ID corresponding to the authenticated user.
        /// </summary>
        public static string GetPrimaryBlog([NotNull] JObject user) => user.Value<string>("primary_blog");

        /// <summary>
        /// Gets the profile URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileUrl([NotNull] JObject user) => user.Value<string>("profile_URL");

        /// <summary>
        /// Gets the username corresponding to the authenticated user.
        /// </summary>
        public static string GetUsername([NotNull] JObject user) => user.Value<string>("username");
    }
}