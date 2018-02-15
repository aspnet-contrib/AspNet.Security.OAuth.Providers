/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Strava
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Strava after a successful authentication process.
    /// </summary>
    public static class StravaAuthenticationHelper
    {
        /// <summary>
        /// Gets the city corresponding to the authenticated user.
        /// </summary>
        public static string GetCity([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("city");
        }

        /// <summary>
        /// Gets the creation date corresponding to the authenticated user.
        /// </summary>
        public static string GetCreationDate([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("created_at");
        }

        /// <summary>
        /// Gets the country corresponding to the authenticated user.
        /// </summary>
        public static string GetCountry([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("country");
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

            return user.Value<string>("email");
        }

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        public static string GetFirstName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("firstname");
        }

        /// <summary>
        /// Gets the gender corresponding to the authenticated user.
        /// </summary>
        public static string GetGender([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("sex");
        }

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        public static string GetLastName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("lastname");
        }

        /// <summary>
        /// Gets the modification date corresponding to the authenticated user.
        /// </summary>
        public static string GetModificationDate([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("updated_at");
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
        /// Gets the premium corresponding to the authenticated user.
        /// </summary>
        public static string GetPremium([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("premium");
        }

        /// <summary>
        /// Gets the profile corresponding to the authenticated user.
        /// </summary>
        public static string GetProfile([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("profile");
        }

        /// <summary>
        /// Gets the profile medium corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileMedium([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("profile_medium");
        }

        /// <summary>
        /// Gets the state corresponding to the authenticated user.
        /// </summary>
        public static string GetState([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("state");
        }

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
    }
}
