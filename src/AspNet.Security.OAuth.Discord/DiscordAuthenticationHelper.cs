/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Discord {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Discord after a successful authentication process.
    /// </summary>
    public static class DiscordAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the username corresponding to the authenticated user.
        /// </summary>
        public static string GetUsername([NotNull] JObject user) => user.Value<string>("username");

        /// <summary>
        /// Gets the email corresponding to the authenticated user.
        /// Note: This requires the 'email' scope to be added under the discord provider in your startup.cs
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("email");
    }
}
