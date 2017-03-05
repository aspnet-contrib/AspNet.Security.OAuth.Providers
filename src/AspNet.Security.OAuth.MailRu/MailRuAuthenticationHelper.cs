/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.MailRu
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from MailRu after a successful authentication process.
    /// </summary>
    public class MailRuAuthenticationHelper
    {
        /// <summary>
        /// Gets the family name corresponding to the authenticated user.
        /// </summary>
        public static string GetFamilyName([NotNull] JObject payload) => payload.Value<string>("last_name");

        /// <summary>
        /// Gets the given name corresponding to the authenticated user.
        /// </summary>
        public static string GetGivenName([NotNull] JObject payload) => payload.Value<string>("first_name");

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject payload) => payload.Value<string>("uid");

        /// <summary>
        /// Gets the nickname corresponding to the authenticated user.
        /// </summary>
        public static string GetNickname([NotNull] JObject payload) => payload.Value<string>("nick");

        /// <summary>
        /// Gets the email corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject payload) => payload.Value<string>("email");

        /// <summary>
        /// Gets the profile link corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject payload) => payload.Value<string>("link");
    }
}