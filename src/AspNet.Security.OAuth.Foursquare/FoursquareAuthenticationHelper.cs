/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Foursquare
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Foursquare after a successful authentication process.
    /// </summary>
    public static class FoursquareAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated payload.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("id");
        }

        /// <summary>
        /// Gets the last name associated with the authenticated payload.
        /// </summary>
        public static string GetLastName([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("lastName");
        }

        /// <summary>
        /// Gets the first name associated with the authenticated payload.
        /// </summary>
        public static string GetFirstName([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("firstName");
        }

        /// <summary>
        /// Gets the payloadname associated with the authenticated payload.
        /// </summary>
        public static string GetUserName([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return $"{GetFirstName(payload)} {GetLastName(payload)}";
        }

        /// <summary>
        /// Gets the gender associated with the authenticated payload.
        /// </summary>
        public static string GetGender([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("gender");
        }

        /// <summary>
        /// Gets the canonical URL corresponding to the authenticated payload.
        /// </summary>
        public static string GetCanonicalUrl([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("canonicalUrl");
        }

        /// <summary>
        /// Gets the home city associated with the authenticated payload.
        /// </summary>
        public static string GetHomeCity([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")
                         ?.Value<JObject>("payload")?.Value<string>("homeCity");
        }

        /// <summary>
        /// Gets the email associated with the authenticated payload.
        /// </summary>
        public static string GetContactEmail([NotNull] JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return payload.Value<JObject>("response")?.Value<JObject>("payload")
                         ?.Value<JObject>("contact")?.Value<string>("email");
        }
    }
}
