/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Patreon after a successful authentication process.
    /// </summary>
    public static class PatreonAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetUserIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?.Value<string>("id");
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?["attributes"]?.Value<string>("full_name");
        }

        /// <summary>
        /// Gets the page URL corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?["attributes"]?.Value<string>("url");
        }

        /// <summary>
        /// Gets the avatar URL corresponding to the authenticated user.
        /// </summary>
        public static string GetAvatarLink([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?["attributes"]?.Value<string>("thumb_url");
        }
    }
}
