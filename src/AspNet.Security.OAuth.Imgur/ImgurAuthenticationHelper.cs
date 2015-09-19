/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Imgur {
    using System.Globalization;

    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Imgur after a successful authentication process.
    /// </summary>
    public static class ImgurAuthenticationHelper {
        /// <summary>Get the id of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The id of the authenticated user.</returns>
        public static string GetId([NotNull] JObject user) => GetString(user, "id");

        /// <summary>Get the url (name) of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The url (name) of the authenticated user.</returns>
        public static string GetUrl([NotNull] JObject user) => GetString(user, "url");

        /// <summary>Get the bio of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The bio of the authenticated user.</returns>
        public static string GetBio([NotNull] JObject user) => GetString(user, "bio");

        /// <summary>Get the reputation of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The reputation of the authenticated user.</returns>
        /// <remarks></remarks>
        public static string GetReputation([NotNull] JObject user) => GetString(user, "reputation");

        /// <summary>Get the epoch time of account creation of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The epoch time of account creation of the authenticated user.</returns>
        /// <remarks></remarks>
        public static string GetCreated([NotNull] JObject user) => GetString(user, "created");

        /// <summary>Get the epoch time of the expiration of the pro account of the authenticated user.</summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The the epoch time of the expiration of the pro account of the authenticated user.</returns>
        /// <remarks></remarks>
        public static string GetProExpiration([NotNull] JObject user) => GetString(user, "pro_expiration");

        private static string GetString([NotNull] JToken token, string key) {
            return token.Value<JObject>("data").Value<string>(key);
        }
    }
}
