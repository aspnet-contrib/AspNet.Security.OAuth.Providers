/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.LinkedIn {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from LinkedIn after a successful authentication process.
    /// </summary>
    public class LinkedInAuthenticationHelper {
        /// <summary>
        /// Gets the email address corresponding to the authenticated user.
        /// </summary>
        public static string GetEmail([NotNull] JObject user) => user.Value<string>("emailAddress");

        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetIdentifier([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetName([NotNull] JObject user) => user.Value<string>("formattedName");

        /// <summary>
        /// Gets the first name corresponding to the authenticated user.
        /// </summary>
        public static string GetGivenName([NotNull] JObject user) => user.Value<string>("firstName");

        /// <summary>
        /// Gets the last name corresponding to the authenticated user.
        /// </summary>
        public static string GetFamilyName([NotNull] JObject user) => user.Value<string>("lastName");

        /// <summary>
        /// Gets the maiden name corresponding to the authenticated user.
        /// </summary>
        public static string GetMaidenName([NotNull] JObject user) => user.Value<string>("maidenName");

        /// <summary>
        /// Gets the phonetic first name corresponding to the authenticated user.
        /// </summary>
        public static string GetPhoneticFirstName([NotNull] JObject user) => user.Value<string>("phoneticFirstName");

        /// <summary>
        /// Gets the phonetic last name corresponding to the authenticated user.
        /// </summary>
        public static string GetPhoneticLastName([NotNull] JObject user) => user.Value<string>("phoneticLastName");

        /// <summary>
        /// Gets the phonetic name corresponding to the authenticated user.
        /// </summary>
        public static string GetPhoneticName([NotNull] JObject user) => user.Value<string>("formattedPhoneticName");

        /// <summary>
        /// Gets the profile picture URL corresponding to the authenticated user.
        /// </summary>
        public static string GetProfilePictureUrl([NotNull] JObject user) => user.Value<string>("pictureUrl");

        /// <summary>
        /// A URL to the member's original unformatted profile picture. This image is usually larger than the picture-url value above.
        /// </summary>
        public static string GetPictureUrls([NotNull] JObject user) => user["pictureUrls"].ToString();

        /// <summary>
        /// Gets the public profile URL corresponding to the authenticated user.
        /// </summary>
        public static string GetPublicProfileUrl([NotNull] JObject user) => user.Value<string>("publicProfileUrl");

        /// <summary>
        /// Gets the industry corresponding to the authenticated user.
        /// https://developer.linkedin.com/docs/reference/industry-codes
        /// </summary>
        public static string GetIndustry([NotNull] JObject user) => user.Value<string>("industry");

        /// <summary>
        /// Gets the summary corresponding to the authenticated user.
        /// </summary>
        public static string GetSummary([NotNull] JObject user) => user.Value<string>("summary");

        /// <summary>
        /// Gets the headline corresponding to the authenticated user.
        /// </summary>
        public static string GetHeadline([NotNull] JObject user) => user.Value<string>("headline");

        /// <summary>
        /// Gets the specialties corresponding to the authenticated user.
        /// </summary>
        public static string GetSpecialties([NotNull] JObject user) => user.Value<string>("specialties");

        /// <summary>
        /// Gets the location object corresponding to the authenticated user.
        /// https://developer.linkedin.com/docs/fields/location
        /// </summary>
        public static string GetLocation([NotNull] JObject user) => user["location"].ToString();

        /// <summary>
        /// Gets the most recent item the member has shared on LinkedIn. If the member has not shared anything, their 'status' is returned instead.
        /// </summary>
        public static string GetCurrentShare([NotNull] JObject user) => user.Value<string>("currentShare");

        /// <summary>
        /// Gets the positions object corresponding to the authenticated user.
        /// https://developer.linkedin.com/docs/fields/positions
        /// </summary>
        public static string GetPositions([NotNull] JObject user) => user["positions"].ToString();

        /// <summary>
        /// The number of LinkedIn connections the member has, capped at 500. See 'num-connections-capped' to determine if the value returned has been capped.
        /// </summary>
        public static string GetNumConnections([NotNull] JObject user) => user.Value<string>("numConnections");

        /// <summary>
        /// Returns 'true' if the member's 'num-connections' value has been capped at 500', or 'false' if 'num-connections' represents the user's true value.
        /// </summary>
        public static string GetNumConnectionsCapped([NotNull] JObject user) => user.Value<string>("numConnectionsCapped");

        /// <summary>
        /// A URL representing the resource you would request for programmatic access to the member's profile.
        /// </summary>
        public static string GetApiStandardProfileRequest([NotNull] JObject user) => user.Value<string>("apiStandardProfileRequest");

        /// <summary>
        /// The URL to the member's authenticated profile on LinkedIn.  You must be logged into LinkedIn to view this URL.
        /// </summary>
        public static string GetSiteStandardProfileRequest([NotNull] JObject user) => user.Value<string>("siteStandardProfileRequest");
    }
}