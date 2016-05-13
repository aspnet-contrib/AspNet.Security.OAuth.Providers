/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Salesforce after a successful authentication process.
    /// </summary>
    public static class SalesforceAuthenticationHelper
    {
        /// <summary>
        /// Gets the Salesforce ID corresponding to the authenticated user.
        /// </summary>
        public static string GetUserIdentifier([NotNull] JObject user) => user.Value<string>("user_id");

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public static string GetUserName([NotNull] JObject user) => user.Value<string>("user_name");
    }
}
