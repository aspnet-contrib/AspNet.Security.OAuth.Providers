/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Gitter
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from GitHub after a successful authentication process.
    /// </summary>
    public static class GitterAuthenticationHelper
    {
        /// <summary>
        /// Gets the username corresponding to the authenticated user.
        /// </summary>
        public static string GetUsername([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("username");
        }

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("id");
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

            return user.Value<string>("displayName");
        }

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("url");
        }

        /// <summary>
        /// Gets the small url avatar corresponding to the authenticated user.
        /// </summary>
        public static string GetAvatarUrlSmall([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("avatarUrlSmall");
        }

        /// <summary>
        /// Gets the medium url avatar corresponding to the authenticated user.
        /// </summary>
        public static string GetAvatarUrlMedium([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("avatarUrlMedium");
        }
    }
}