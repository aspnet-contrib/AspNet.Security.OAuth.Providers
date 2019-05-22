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
            public const string PictureUrl = "urn:linkedin:pictureurl";
            public const string PictureUrls = "urn:linkedin:pictureurls";
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
        }
    }
}