/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Paypal
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Paypal after a successful authentication process.
    /// </summary>
    public static class PaypalAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("user_id")
                                                                         ?.Split('/')
                                                                         ?.LastOrDefault();

        /// <summary>
        /// Gets the full name corresponding to the authenticated user.
        /// </summary>
        public static string GetFullName([NotNull] JObject user) => user.Value<string>("name");

        /// <summary>
        /// Gets the given name corresponding to the authenticated user.
        /// </summary>
        public static string GetGivenName([NotNull] JObject user) => user.Value<string>("given_name");

        /// <summary>
        /// Gets the family name corresponding to the authenticated user.
        /// </summary>
        public static string GetFamilyName([NotNull] JObject user) => user.Value<string>("family_name");

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("email");

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject user) => user.Value<string>("user_id");

    }
}
