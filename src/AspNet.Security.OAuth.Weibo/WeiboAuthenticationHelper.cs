/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Weibo
{
    /// <summary>
    /// Contains static methods that allow to extract user information from a <see cref="JObject"/>
    /// instance retrieved from Weibo after a successful authentication process.
    /// </summary>
    public class WeiboAuthenticationHelper
    {
        /// <summary>
        /// Gets the user identifier.
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
        /// Gets the user screen name (display name).
        /// </summary>
        public static string GetScreenName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("screen_name");
        }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public static string GetName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("name");
        }

        /// <summary>
        /// Gets the user profile image url.
        /// </summary>
        public static string GetProfileImageUrl([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("profile_image_url");
        }

        /// <summary>
        /// Gets the user gender.
        /// </summary>
        public static string GetGender([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("gender");
        }

        /// <summary>
        /// Gets the user avatar (large image).
        /// </summary>
        public static string GetAvatarLarge([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("avatar_large");
        }

        /// <summary>
        /// Gets the user avatar (HD image).
        /// </summary>
        public static string GetAvatarHD([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("avatar_hd");
        }

        /// <summary>
        /// Gets the user cover image phone.
        /// </summary>
        public static string GetCoverImagePhone([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("cover_image_phone");
        }

        /// <summary>
        /// Gets the user location.
        /// </summary>
        public static string GetLocation([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("location");
        }
    }
}
