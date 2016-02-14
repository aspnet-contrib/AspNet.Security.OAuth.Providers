/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Yahoo {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Yahoo after a successful authentication process.
    /// </summary>
    public class YahooAuthenticationHelper {
        /// <summary>
        /// Gets the family name corresponding to the authenticated user.
        /// </summary>
        public static string GetFamilyName([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                               ?.Value<string>("familyName");

        /// <summary>
        /// Gets the given name corresponding to the authenticated user.
        /// </summary>
        public static string GetGivenName([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                              ?.Value<string>("givenName");

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                               ?.Value<string>("guid");

        /// <summary>
        /// Gets the nickname corresponding to the authenticated user.
        /// </summary>
        public static string GetNickname([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                             ?.Value<string>("nickname");

        /// <summary>
        /// Gets the profile image URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileImageUrl([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                                    ?.Value<JObject>("image")
                                                                                    ?.Value<string>("imageUrl");

        /// <summary>
        /// Gets the profile URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileUrl([NotNull] JObject payload) => payload.Value<JObject>("profile")
                                                                               ?.Value<string>("profileUrl");
    }
}