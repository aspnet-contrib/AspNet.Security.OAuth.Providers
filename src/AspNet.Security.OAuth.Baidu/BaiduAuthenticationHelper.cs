/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Baidu
{
    /// <summary>
    /// Contains static methods that allow to extract user information from a <see cref="JObject"/>
    /// instance retrieved from Baidu after a successful authentication process.
    /// </summary>
    public class BaiduAuthenticationHelper
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

            return user.Value<string>("uid");
        }

        /// <summary>
        /// Gets the user display name.
        /// </summary>
        public static string GetDisplayName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("uname");
        }

        /// <summary>
        /// Gets the user portrait.
        /// </summary>
        public static string GetPortrait([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("portrait");
        }
    }
}
