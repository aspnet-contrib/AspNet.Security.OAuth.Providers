/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Imgur {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Imgur after a successful authentication process.
    /// </summary>
    public static class ImgurAuthenticationHelper {
        /// <summary>
        /// Gets the id of the authenticated user.
        /// </summary>
        public static string GetId([NotNull] JObject user) => user.Value<JObject>("data")
                                                                 ?.Value<string>("id");

        /// <summary>
        /// Gets the url (name) of the authenticated user.
        /// </summary>
        public static string GetUrl([NotNull] JObject user) => user.Value<JObject>("data")
                                                                  ?.Value<string>("url");

        /// <summary>
        /// Gets the bio of the authenticated user.
        /// </summary>
        public static string GetBio([NotNull] JObject user) => user.Value<JObject>("data")
                                                                  ?.Value<string>("bio");

        /// <summary>
        /// Gets the reputation of the authenticated user.
        /// </summary>
        public static string GetReputation([NotNull] JObject user) => user.Value<JObject>("data")
                                                                         ?.Value<string>("reputation");

        /// <summary>
        /// Gets the epoch time of account creation of the authenticated user.
        /// </summary>
        public static string GetCreated([NotNull] JObject user) => user.Value<JObject>("data")
                                                                      ?.Value<string>("created");

        /// <summary>
        /// Gets the epoch time of the expiration of the pro account of the authenticated user.
        /// </summary>
        public static string GetProExpiration([NotNull] JObject user) => user.Value<JObject>("data")
                                                                            ?.Value<string>("pro_expiration");
    }
}
