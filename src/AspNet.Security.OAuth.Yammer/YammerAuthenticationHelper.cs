/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Yammer
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Yammer after a successful authentication process.
    /// </summary>
    public static class YammerAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetId([NotNull] JObject payload) => payload.Value<string>("id");

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        public static string GetFirstName([NotNull] JObject payload) => payload.Value<string>("first_name");

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        public static string GetLastName([NotNull] JObject payload) => payload.Value<string>("last_name");

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject payload) => payload.Value<string>("name");

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject payload) => payload.Value<string>("email");

        /// <summary>
        /// Gets the job title corresponding to the authenticated user.
        /// </summary>
        public static string GetJobTitle([NotNull] JObject payload) => payload.Value<string>("job_title");

        /// <summary>
        /// Gets the web link corresponding to the authenticated user.
        /// </summary>
        public static string GetLink([NotNull] JObject payload) => payload.Value<string>("web_url");
    }
}