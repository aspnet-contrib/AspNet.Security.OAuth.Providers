/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.EVEOnline {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from EVEOnline after a successful authentication process.
    /// </summary>
    public static class EVEOnlineAuthenticationHelper {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated character.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("CharacterID");

        /// <summary>
        /// Gets the name corresponding to the authenticated character.
        /// </summary>
        public static string GetName([NotNull] JObject user) => user.Value<string>("CharacterName");

        /// <summary>
        /// Gets the access token expiration date.
        /// </summary>
        public static string GetTokenExpiration([NotNull] JObject user) => user.Value<string>("ExpiresOn");

        /// <summary>
        /// Gets the scopes associated with the access token.
        /// </summary>
        public static string GetTokenScopes([NotNull] JObject user) => user.Value<string>("Scopes");
    }
}
