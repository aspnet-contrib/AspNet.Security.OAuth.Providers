/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Imgur {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Imgur after a successful authentication process.
    /// </summary>
    public static class ImgurAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<JObject>("data").Value<string>("id");
        
        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject user) => user.Value<JObject>("data").Value<string>("url");
    }
}
