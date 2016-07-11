/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
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
        public static string GetIdentifier([NotNull] JObject characterInfo) => characterInfo.Value<long>("CharacterID").ToString();

        /// <summary>
        /// Gets the name corresponding to the authenticated character.
        /// </summary>
        public static string GetName([NotNull] JObject characterInfo) => characterInfo.Value<string>("CharacterName");

        /// <summary>
        /// Gets the account expiration date corresponding to the authenticated character.
        /// </summary>
        public static string GetExpiration([NotNull] JObject characterInfo) => characterInfo.Value<string>("ExpiresOn");

        /// <summary>
        /// Gets scopes corresponding to the authenticated character.
        /// </summary>
        public static string GetScopes([NotNull] JObject characterInfo) => characterInfo.Value<string>("Scopes");
    }
}
