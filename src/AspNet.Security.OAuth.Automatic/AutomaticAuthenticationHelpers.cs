/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Automatic {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Automatic after a successful authentication process.
    /// </summary>
    public static class AutomaticHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        public static string GetGivenName([NotNull] JObject user) => user.Value<string>("first_name");

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        public static string GetFamilyName([NotNull] JObject user) => user.Value<string>("last_name");

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetLogin([NotNull] JObject user) => user.Value<string>("username");

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("email");

        /// <summary>
        /// Gets the url corresponding to the authenticated user.
        /// </summary>
        public static string GetUrl([NotNull] JObject user) => user.Value<string>("url");
    }
}