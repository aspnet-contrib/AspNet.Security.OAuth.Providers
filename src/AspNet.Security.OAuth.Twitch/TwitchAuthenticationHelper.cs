﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Twitch
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Twitch after a successful authentication process.
    /// </summary>
    public static class TwitchAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?[0]?["id"]?.ToObject<string>();
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?[0]?["login"]?.ToObject<string>();
        }

        /// <summary>
        /// Gets the display name corresponding to the authenticated user.
        /// </summary>
        public static string GetDisplayName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?[0]?["display_name"]?.ToObject<string>();
        }

        /// <summary>
        /// Gets the email corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["data"]?[0]?["email"]?.ToObject<string>();
        }
    }
}
