/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.MailChimp {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from MailChimp after a successful authentication process.
    /// </summary>
    public static class MailChimpAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject payload) => payload.Value<string>("user_id");

        /// <summary>
        /// Gets the data center corresponding to the authenticated user.
        /// </summary>
        public static string GetDataCenter([NotNull] JObject payload) => payload.Value<string>("dc");

        /// <summary>
        /// Gets the account name corresponding to the authenticated user.
        /// </summary>
        public static string GetAccountName([NotNull] JObject payload) => payload.Value<string>("accountname");

        /// <summary>
        /// Gets the role corresponding to the authenticated user.
        /// </summary>
        public static string GetRole(JObject payload) => payload.Value<string>("role");

        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject payload) => payload.Value<JObject>("login")
                                                                          ?.Value<string>("email");

        /// <summary>
        /// Gets the login id corresponding to the authenticated user.
        /// </summary>
        public static string GetLoginId([NotNull] JObject payload) => payload.Value<JObject>("login")
                                                                            ?.Value<string>("login_id");
        
        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject payload) => payload.Value<JObject>("login")
                                                                         ?.Value<string>("login_name");

        /// <summary>
        /// Gets the login email address corresponding to the authenticated user.
        /// </summary>
        public static string GetLoginEmail([NotNull] JObject payload) => payload.Value<JObject>("login")
                                                                               ?.Value<string>("login_email");

        /// <summary>
        /// Gets the login url address corresponding to the authenticated user.
        /// </summary>
        public static string GetLoginUrl([NotNull] JObject payload) => payload.Value<string>("login_url");

        /// <summary>
        /// Gets the api endpoint address corresponding to the authenticated user.
        /// </summary>
        public static string GetApiEndPoint([NotNull] JObject payload) => payload.Value<string>("api_endpoint");
    }
}
