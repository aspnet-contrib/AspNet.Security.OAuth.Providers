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
        public static string GetId([NotNull] JObject user) => GetString(user, "id");

        public static string GetUrl([NotNull] JObject user) => GetString(user, "url");

        public static string GetBio([NotNull] JObject user) => GetString(user, "bio");

        public static string GetReputation([NotNull] JObject user) => GetString(user, "reputation");

        public static string GetCreated([NotNull] JObject user) => GetString(user, "created");

        public static string GetProExpiration([NotNull] JObject user) => GetString(user, "pro_expiration");

        private static string GetString([NotNull] JToken token, string key) {
            return token.Value<JObject>("data").Value<string>(key);
        }
    }
}
