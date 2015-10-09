/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Internal;
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
        public static string GetIdentifier([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                               ?.Value<JObject>("user")
                                                                               ?.Value<string>("id");

        /// <summary>
        /// Gets the last name associated with the authenticated user.
        /// </summary>
        public static string GetLastName([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                             ?.Value<JObject>("user")
                                                                             ?.Value<string>("lastName");

        /// <summary>
        /// Gets the first name associated with the authenticated user.
        /// </summary>
        public static string GetFirstName([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                              ?.Value<JObject>("user")
                                                                              ?.Value<string>("firstName");

        /// <summary>
        /// Gets the username associated with the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject payload) => $"{GetFirstName(payload)} {GetLastName(payload)}";

        /// <summary>
        /// Gets the gender associated with the authenticated user.
        /// </summary>
        public static string GetGender([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                           ?.Value<JObject>("user")
                                                                           ?.Value<string>("gender");

        /// <summary>
        /// Gets the canonical URL corresponding to the authenticated user.
        /// </summary>
        public static string GetCanonicalUrl([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                                 ?.Value<JObject>("user")
                                                                                 ?.Value<string>("canonicalUrl");

        /// <summary>
        /// Gets the home city associated with the authenticated user.
        /// </summary>
        public static string GetHomeCity([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                             ?.Value<JObject>("user")
                                                                             ?.Value<string>("homeCity");

        /// <summary>
        /// Gets the email associated with the authenticated user.
        /// </summary>
        public static string GetContactEmail([NotNull] JObject payload) => payload.Value<JObject>("response")
                                                                                 ?.Value<JObject>("user")
                                                                                 ?.Value<JObject>("contact")
                                                                                 ?.Value<string>("email");
    }
}
