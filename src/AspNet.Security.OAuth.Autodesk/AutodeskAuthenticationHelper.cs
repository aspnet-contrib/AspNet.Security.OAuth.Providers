/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Autodesk after a successful authentication process.
    /// </summary>
    public static class AutodeskAuthenticationHelper
    {
        /// <summary>
        /// Gets the unique user identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetUserId([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("userId");
        }

        /// <summary>
        /// Gets the username corresponding to the authenticated user (e.g. John Doe or john.doe)
        /// </summary>
        public static string GetUserName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("userName");
        }

        /// <summary>
        /// Gets the email address associated with the authenticated user (e.g. john.doe@autodesk.com)
        /// </summary>
        public static string GetEmailAddress([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("emailId");
        }

        /// <summary>
        /// Gets the first name of the authenticated user (e.g. John)
        /// </summary>
        public static string GetFirstName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("firstName");
        }

        /// <summary>
        /// Gets the last name of the authenticated user (e.g. Doe)
        /// </summary>
        public static string GetLastName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("lastName");
        }

        /// <summary>
        /// Gets a boolean value indicating whether or not the user's email address has been verified.
        /// </summary>
        public static string GetEmailVerified([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("emailVerified");
        }

        /// <summary>
        /// Gets a boolean value indicating whether or not the user has enabled two-factor authentication.
        /// </summary>
        public static string GetTwoFactorEnabled([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("2FaEnabled");
        }
    }
}
