/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Slack
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Slack after a successful authentication process.
    /// </summary>
    public static class SlackAuthenticationHelper
    {
        /// <summary>
        /// Returns a unique identifer for the authenticated user.
        /// </summary>
        /// <remarks>
        /// According to the Slack API documentation at https://api.slack.com/methods/users.identity, user IDs are
        /// not guaranteed to be globally unique across all Slack users.  The combination of user ID and team ID, 
        /// on the other hand, is guaranteed to be globally unique.
        /// </remarks>
        public static string GetUniqueIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return $"{GetTeamIdentifier(user)}|{GetUserIdentifier(user)}";
        }

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetUserEmail([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<JObject>("user")?.Value<string>("email");
        }

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetUserIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<JObject>("user")?.Value<string>("id");
        }

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<JObject>("user")?.Value<string>("name");
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<JObject>("team")?.Value<string>("id");
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<JObject>("team")?.Value<string>("name");
        }
    }
}
