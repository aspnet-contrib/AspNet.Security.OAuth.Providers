/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.StackExchange {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from StackExchange after a successful authentication process.
    /// </summary>
    public static class StackExchangeAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user["items"]?[0]?.Value<string>("account_id");

        /// <summary>
        /// Gets the display name corresponding to the authenticated user.
        /// </summary>
        public static string GetDisplayName([NotNull] JObject user) => user["items"]?[0]?.Value<string>("display_name");

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject user) => user["items"]?[0]?.Value<string>("link");

        /// <summary>
        /// Gets the website URL associated with the authenticated user.
        /// </summary>
        public static string GetWebsiteUrl([NotNull] JObject user) => user["items"]?[0]?.Value<string>("website_url");
    }
}
