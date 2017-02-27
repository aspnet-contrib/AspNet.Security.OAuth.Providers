/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

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
        /// Unique user identifier
        /// </summary>
        /// <remarks>
        /// Usually alphanumeric. Some older IDs may reflect username.
        /// </remarks>
        /// <param name="user">User info JSON</param>
        /// <returns>User Id</returns>
        public static string GetUserId(JObject user) => user.Value<string>("userId");

        /// <summary>
        /// Username e.g. John Doe or john.doe
        /// </summary>
        /// <remarks>
        /// Usually the content of the email address before the @ symbol.
        /// Can be used for sign-in.
        /// </remarks>
        /// <param name="user">User info JSON</param>
        /// <returns>User name</returns>
        public static string GetUserName(JObject user) => user.Value<string>("userName");

        /// <summary>
        /// User email address e.g. john.doe@autodesk.com
        /// </summary>
        /// <remarks>
        /// Directly supplied by the user at signup.
        /// Can be used for sign-in.
        /// </remarks>
        /// <param name="user">User info JSON</param>
        /// <returns>Email address</returns>
        public static string GetEmailAddress(JObject user) => user.Value<string>("emailId");

        /// <summary>
        /// User first name e.g. John
        /// </summary>
        /// <remarks>
        /// Directly supplied by the user at signup.
        /// </remarks>
        /// <param name="user">User info JSON</param>
        /// <returns>First name</returns>
        public static string GetFirstName(JObject user) => user.Value<string>("firstName");

        /// <summary>
        /// User last name e.g. Doe
        /// </summary>
        /// <remarks>
        /// Directly supplied by the user at signup.
        /// </remarks>
        /// <param name="user">User info JSON</param>
        /// <returns>Last name</returns>
        public static string GetLastName(JObject user) => user.Value<string>("lastName");

        /// <summary>
        /// Boolean value indicating whether or not the user's email address has been verified
        /// </summary>
        /// <param name="user">User info JSON</param>
        /// <returns>Status of email verification</returns>
        public static string GetEmailVerified(JObject user) => user.Value<string>("emailVerified");

        /// <summary>
        /// Boolean value indicating whether or not the user has enabled two-factor authentication
        /// </summary>
        /// <param name="user">User info JSON</param>
        /// <returns>Status of 2FA</returns>
        public static string GetTwoFactorEnabled(JObject user) => user.Value<string>("2FaEnabled");
    }
}
