/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Foursquare {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Foursquare after a successful authentication process.
    /// </summary>
    public static class FoursquareAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("id");

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetLastName([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("lastName");

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetFirstName([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("firstName");

        /// <summary>
        /// Gets the username corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserName([NotNull] JObject payload) => string.Format("{0} {1}", GetFirstName(payload), GetLastName(payload));

        /// <summary>
        /// Gets the gender corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetGender([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("gender");

        /// <summary>
        /// Gets the GetCanonicalUrl corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetCanonicalUrl([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("canonicalUrl");

        /// <summary>
        /// Gets the home city corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetHomeCity([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<string>("homeCity");

        /// <summary>
        /// Gets the email corresponding to the authenticated user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetContactEmail([NotNull] JObject payload) => payload.Value<JObject>("response")?.Value<JObject>("user")?.Value<JObject>("contact")?.Value<string>("email");
    }
}
