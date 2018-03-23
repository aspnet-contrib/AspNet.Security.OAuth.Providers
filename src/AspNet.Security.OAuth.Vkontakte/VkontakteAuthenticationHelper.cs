/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Vkontakte
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Vkontakte after a successful authentication process.
    /// </summary>
    public static class VkontakteAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier associated with the logged in user.
        /// </summary>
        public static string GetId([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("id");
        }

        /// <summary>
        /// Gets the hash for checking authorization on the remote client.
        /// </summary>
        public static string GetHash([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("hash");
        }

        /// <summary>
        /// Gets the email address associated with the logged in user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("email");
        }

        /// <summary>
        /// Gets the first name associated with the logged in user.
        /// </summary>
        public static string GetFirstName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("first_name");
        }

        /// <summary>
        /// Gets the last name associated with the logged in user.
        /// </summary>
        public static string GetLastName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("last_name");
        }

        /// <summary>
        /// Gets the URL of the user profile picture.
        /// </summary>
        public static string GetPhoto([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("photo");
        }

        /// <summary>
        /// Gets the URL of the user profile thumbnail.
        /// </summary>
        public static string GetPhotoThumbnail([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("photo_rec");
        }
    }
}