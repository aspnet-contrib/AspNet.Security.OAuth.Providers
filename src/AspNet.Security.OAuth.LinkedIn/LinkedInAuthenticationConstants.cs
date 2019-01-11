/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.LinkedIn
{
    /// <summary>
    /// Contains constants specific to the <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public static class LinkedInAuthenticationConstants
    {
        public static class Claims
        {
            public const string CurrentShare = "urn:linkedin:currentshare";
            public const string FormattedPhoneticName = "urn:linkedin:phoneticname";
            public const string Headline = "urn:linkedin:headline";
            public const string Industry = "urn:linkedin:industry";
            public const string Location = "urn:linkedin:location";
            public const string MaidenName = "urn:linkedin:maidenname";
            public const string NumConnections = "urn:linkedin:numconnections";
            public const string NumConnectionsCapped = "urn:linkedin:numconnectionscapped";
            public const string PhoneticFirstName = "urn:linkedin:phoneticfirstname";
            public const string PhoneticLastName = "urn:linkedin:phoneticlastname";
            public const string PictureUrl = "urn:linkedin:pictureurl";
            public const string PictureUrls = "urn:linkedin:pictureurls";
            public const string Positions = "urn:linkedin:positions";
            public const string ProfileUrl = "urn:linkedin:profile";
            public const string Specialties = "urn:linkedin:specialties";
            public const string Summary = "urn:linkedin:summary";
        }

        public const string EmailAddressField = "email-address";

        // https://developer.linkedin.com/docs/fields/basic-profile
        public static class ProfileFields
        {
            /// <summary>
            /// A unique identifying value for the member.
            ///This value is linked to your specific application.Any attempts to use it with a different application will result in a "404 - Invalid member id" error.
            /// </summary>
            public const string Id = "id";
            /// <summary>
            /// The member's first name.
            /// </summary>
            public const string FirstName = "first-name";
            /// <summary>
            /// The member's last name.
            /// </summary>
            public const string LastName = "last-name";
            /// <summary>
            /// The member's name, formatted based on language.
            /// </summary>
            public const string FormattedName = "formatted-name";

            /// <summary>
            /// The member's maiden name.
            /// </summary>
            public const string MaidenName = "maiden-name";
            /// <summary>
            /// The member's first name, spelled phonetically.
            /// </summary>
            public const string PhoneticFirstName = "phonetic-first-name";
            /// <summary>
            /// The member's last name, spelled phonetically.
            /// </summary>
            public const string PhoneticLastName = "phonetic-last-name";
            /// <summary>
            /// The member's name, spelled phonetically and formatted based on language.
            /// </summary>
            public const string FormattedPhoneticName = "formatted-phonetic-name";
            /// <summary>
            /// The member's headline.
            /// </summary>
            public const string Headline = "headline";
            /// <summary>
            /// An object representing the user's physical location.
            /// See Location Fields for a description of the fields available within this object.
            /// </summary>
            public const string Location = "location";
            /// <summary>
            /// The industry the member belongs to.
            /// See <a href="https://developer.linkedin.com/docs/reference/industry-codes">Industry Codes</a> for a list of possible values.
            /// </summary>
            public const string Industry = "industry";
            /// <summary>
            /// The most recent item the member has shared on LinkedIn.
            /// If the member has not shared anything, their 'status' is returned instead.
            /// </summary>
            public const string CurrentShare = "current-share";
            /// <summary>
            /// The number of LinkedIn connections the member has, capped at 500.
            /// See 'num-connections-capped' to determine if the value returned has been capped.
            /// </summary>
            public const string NumConnections = "num-connections";
            /// <summary>
            /// Returns 'true' if the member's 'num-connections' value has been capped at 500',
            /// or 'false' if 'num-connections' represents the user's true value.
            /// </summary>
            public const string NumConnectionCapped = "num-connections-capped";
            /// <summary>
            /// A long-form text area describing the member's professional profile.
            /// </summary>
            public const string Summary = "summary";
            /// <summary>
            /// A short-form text area describing the member's specialties.
            /// </summary>
            public const string Specialties = "specialties";
            /// <summary>
            /// An object representing the member's current position.
            /// See <a href="https://developer.linkedin.com/docs/fields/positions">Position Fields</a> for a description of the fields available within this object.
            /// </summary>
            public const string Positions = "positions";
            /// <summary>
            /// A URL to the member's formatted profile picture, if one has been provided.
            /// </summary>
            public const string PictureUrl = "picture-url";
            /// <summary>
            /// A URL to the member's original unformatted profile picture.
            /// This image is usually larger than the picture-url value above.
            /// </summary>
            public const string PictureUrlsOriginal = "picture-urls::(original)";
            /// <summary>
            /// The URL to the member's authenticated profile on LinkedIn.
            /// You must be logged into LinkedIn to view this URL.
            /// </summary>
            public const string SiteStandardProfileRequest = "site-standard-profile-request";
            /// <summary>
            /// A URL representing the resource you would request for programmatic access to the member's profile.
            /// </summary>
            public const string ApiStandardProfileRequest = "api-standard-profile-request";
            /// <summary>
            /// The URL to the member's public profile on LinkedIn.
            /// </summary>
            public const string PublicProfileUrl = "public-profile-url";
        }
    }
}
