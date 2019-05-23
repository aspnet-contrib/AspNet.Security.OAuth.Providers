/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;

namespace AspNet.Security.OAuth.LinkedIn
{
    /// <summary>
    /// Contains constants specific to the <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public static class LinkedInAuthenticationConstants
    {
        private const string ObsoleteMessage = "This constant is not used in the LinkedIn v2.0 API.";

        public static class Claims
        {
            public const string PictureUrl = "urn:linkedin:pictureurl";

            public const string PictureUrls = "urn:linkedin:pictureurls";

            [Obsolete(ObsoleteMessage)]
            public const string CurrentShare = "urn:linkedin:currentshare";

            [Obsolete(ObsoleteMessage)]
            public const string FormattedPhoneticName = "urn:linkedin:phoneticname";

            [Obsolete(ObsoleteMessage)]
            public const string Headline = "urn:linkedin:headline";

            [Obsolete(ObsoleteMessage)]
            public const string Industry = "urn:linkedin:industry";

            [Obsolete(ObsoleteMessage)]
            public const string Location = "urn:linkedin:location";

            [Obsolete(ObsoleteMessage)]
            public const string MaidenName = "urn:linkedin:maidenname";

            [Obsolete(ObsoleteMessage)]
            public const string NumConnections = "urn:linkedin:numconnections";

            [Obsolete(ObsoleteMessage)]
            public const string NumConnectionsCapped = "urn:linkedin:numconnectionscapped";

            [Obsolete(ObsoleteMessage)]
            public const string PhoneticFirstName = "urn:linkedin:phoneticfirstname";

            [Obsolete(ObsoleteMessage)]
            public const string PhoneticLastName = "urn:linkedin:phoneticlastname";

            [Obsolete(ObsoleteMessage)]
            public const string Positions = "urn:linkedin:positions";

            [Obsolete(ObsoleteMessage)]
            public const string ProfileUrl = "urn:linkedin:profile";

            [Obsolete(ObsoleteMessage)]
            public const string Specialties = "urn:linkedin:specialties";

            [Obsolete(ObsoleteMessage)]
            public const string Summary = "urn:linkedin:summary";
        }

        public const string EmailAddressField = "emailAddress";

        /// <summary>
        /// Available profile fields after a LinkedIn authentication.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/profile/lite-profile?context=linkedin/consumer/context</a>
        /// </summary>
        public static class ProfileFields
        {
            /// <summary>
            /// The unique identifier for the given member. May also be referenced as the <c>personId</c> within a Person URN (<c>urn:li:person:{personId}</c>).
            /// The <c>id</c> is unique to your specific developer application. Any attempts to use the <c>id</c> with other developer applications will not succeed.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// First name of the member. Represented as a MultiLocaleString object type.
            /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// Last name of the member. Represented as a MultiLocaleString object type.
            /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// Metadata about the member's picture in the profile. See Profile Picture Fields for more information.
            /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/profile/profile-picture</a>
            /// </summary>
            public const string PictureUrl = "profilePicture(displayImage~:playableStreams)";

            /// <summary>
            /// The member's name, formatted based on language.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string FormattedName = "formatted-name";

            /// <summary>
            /// The member's maiden name.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string MaidenName = "maiden-name";

            /// <summary>
            /// The member's first name, spelled phonetically.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string PhoneticFirstName = "phonetic-first-name";

            /// <summary>
            /// The member's last name, spelled phonetically.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string PhoneticLastName = "phonetic-last-name";

            /// <summary>
            /// The member's name, spelled phonetically and formatted based on language.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string FormattedPhoneticName = "formatted-phonetic-name";

            /// <summary>
            /// The member's headline.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Headline = "headline";

            /// <summary>
            /// An object representing the user's physical location.
            /// See Location Fields for a description of the fields available within this object.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Location = "location";

            /// <summary>
            /// The industry the member belongs to.
            /// See <a href="https://developer.linkedin.com/docs/reference/industry-codes">Industry Codes</a> for a list of possible values.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Industry = "industry";

            /// <summary>
            /// The most recent item the member has shared on LinkedIn.
            /// If the member has not shared anything, their 'status' is returned instead.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string CurrentShare = "current-share";

            /// <summary>
            /// The number of LinkedIn connections the member has, capped at 500.
            /// See 'num-connections-capped' to determine if the value returned has been capped.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string NumConnections = "num-connections";

            /// <summary>
            /// Returns 'true' if the member's 'num-connections' value has been capped at 500',
            /// or 'false' if 'num-connections' represents the user's true value.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string NumConnectionCapped = "num-connections-capped";

            /// <summary>
            /// A long-form text area describing the member's professional profile.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Summary = "summary";

            /// <summary>
            /// A short-form text area describing the member's specialties.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Specialties = "specialties";

            /// <summary>
            /// An object representing the member's current position.
            /// See <a href="https://developer.linkedin.com/docs/fields/positions">Position Fields</a> for a description of the fields available within this object.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string Positions = "positions";

            /// <summary>
            /// A URL to the member's original unformatted profile picture.
            /// This image is usually larger than the picture-url value above.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string PictureUrlsOriginal = "picture-urls::(original)";

            /// <summary>
            /// The URL to the member's authenticated profile on LinkedIn.
            /// You must be logged into LinkedIn to view this URL.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string SiteStandardProfileRequest = "site-standard-profile-request";

            /// <summary>
            /// A URL representing the resource you would request for programmatic access to the member's profile.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string ApiStandardProfileRequest = "api-standard-profile-request";

            /// <summary>
            /// The URL to the member's public profile on LinkedIn.
            /// </summary>
            [Obsolete(ObsoleteMessage)]
            public const string PublicProfileUrl = "public-profile-url";
        }
    }
}
