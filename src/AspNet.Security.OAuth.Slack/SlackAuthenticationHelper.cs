/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Slack {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Slack after a successful authentication process.
    /// </summary>
    public static class SlackAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetUserIdentifier([NotNull] JObject user) => user.Value<string>("user_id");

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject user) => user.Value<string>("user");

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamIdentifier([NotNull] JObject user) => user.Value<string>("team_id");

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamName([NotNull] JObject user) => user.Value<string>("team");

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamLink([NotNull] JObject user) => user.Value<string>("url");
    }
}
