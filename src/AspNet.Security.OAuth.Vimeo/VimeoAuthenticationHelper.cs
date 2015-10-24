/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Vimeo {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Vimeo after a successful authentication process.
    /// </summary>
    public static class VimeoAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("uri")
                                                                         ?.Split('/')
                                                                         ?.LastOrDefault();

        /// <summary>
        /// Gets the full name corresponding to the authenticated user.
        /// </summary>
        public static string GetFullName([NotNull] JObject user) => user.Value<string>("name");

        /// <summary>
        /// Gets the profile url corresponding to the authenticated user.
        /// </summary>
        public static string GetProfileUrl([NotNull] JObject user) => user.Value<string>("link");
    }
}
