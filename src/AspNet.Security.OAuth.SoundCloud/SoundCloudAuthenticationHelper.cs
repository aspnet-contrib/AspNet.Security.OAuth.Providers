/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.SoundCloud
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from SoundCloud after a successful authentication process.
    /// </summary>
    public static class SoundCloudAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the user name corresponding to the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject user) => user.Value<string>("username");

        /// <summary>
        /// Gets the full name corresponding to the authenticated user.
        /// </summary>
        public static string GetFullName([NotNull] JObject user) => user.Value<string>("full_name");

        /// <summary>
        /// Gets the country corresponding to the authenticated user.
        /// </summary>
        public static string GetCountry([NotNull] JObject user) => user.Value<string>("country");

        /// <summary>
        /// Gets the city corresponding to the authenticated user.
        /// </summary>
        public static string GetCity([NotNull] JObject user) => user.Value<string>("city");

        /// <summary>
        /// Gets the profile url corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileUrl([NotNull] JObject user) => user.Value<string>("permalink_url");
    }
}
