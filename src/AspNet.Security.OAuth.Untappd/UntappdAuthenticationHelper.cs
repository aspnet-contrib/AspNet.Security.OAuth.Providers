/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Untappd {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Untappd after a successful authentication process.
    /// </summary>
    public static class UntappdAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user["user"]?.Value<string>("id");

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetUsername([NotNull] JObject user) => user["user"]?.Value<string>("user_name");

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        public static string GetFirstName([NotNull] JObject user) => user["user"]?.Value<string>("first_name");

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        public static string GetLastName([NotNull] JObject user) => user["user"]?.Value<string>("last_name");

        /// <summary>
        /// Gets the location corresponding to the authenticated user.
        /// </summary>
        public static string GetLocation([NotNull] JObject user) => user["user"]?.Value<string>("location");

        /// <summary>
        /// Gets the url corresponding to the authenticated user.
        /// </summary>
        public static string GetUrl([NotNull] JObject user) => user["user"]?.Value<string>("url");

        /// <summary>
        /// Gets the avatar corresponding to the authenticated user.
        /// </summary>
        public static string GetAvatar([NotNull] JObject user) => user["user"]?.Value<string>("user_avatar");
    }
}
