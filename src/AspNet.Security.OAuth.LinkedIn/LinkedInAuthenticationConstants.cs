/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.LinkedIn;

/// <summary>
/// Contains constants specific to the <see cref="LinkedInAuthenticationHandler"/>.
/// </summary>
public static class LinkedInAuthenticationConstants
{
    public static class Claims
    {
        [Obsolete($"This constant is obsolete and is no longer used. It will be removed in a future version. Use {nameof(Picture)} instead.", false)]
        public const string PictureUrl = "urn:linkedin:pictureurl";

        [Obsolete($"This constant is obsolete and is no longer used. It will be removed in a future version. Use {nameof(Picture)} instead.", false)]
        public const string PictureUrls = "urn:linkedin:pictureurls";

        public const string Picture = "picture";

        public const string Email = "email";

        public const string Sub = "sub";

        public const string EmailVerified = "email_verified";

        public const string Name = "name";

        public const string GivenName = "given_name";

        public const string FamilyName = "family_name";
    }

    public const string EmailAddressField = "emailAddress";

    /// <summary>
    /// Available profile fields after a LinkedIn authentication.
    /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/profile/lite-profile?context=linkedin/consumer/context</a>
    /// </summary>
    public static class ProfileFields
    {
        /// <summary>
        /// The unique identifier for the given member.
        /// </summary>
        public const string Id = "sub";

        /// <summary>
        /// First name of the member. Represented as a MultiLocaleString object type.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
        /// </summary>
        [Obsolete($"This constant is obsolete and is no longer used. It will be removed in a future version. Use {nameof(GivenName)} instead.", false)]
        public const string FirstName = "firstName";

        /// <summary>
        /// Last name of the member. Represented as a MultiLocaleString object type.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
        /// </summary>
        [Obsolete($"This constant is obsolete and is no longer used. It will be removed in a future version. Use {nameof(FamilyName)} instead.", false)]
        public const string LastName = "lastName";

        /// <summary>
        /// Metadata about the member's picture in the profile. See Profile Picture Fields for more information.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/profile/profile-picture</a>
        /// </summary>
        [Obsolete($"This constant is obsolete and is no longer used. It will be removed in a future version. Use {nameof(Picture)} instead.", false)]
        public const string PictureUrl = "profilePicture(displayImage~:playableStreams)";

        /// <summary>
        /// Full name of the member.
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// Picture URL of the member.
        /// </summary>
        public const string Picture = "picture";

        /// <summary>
        /// Given/First name of the member.
        /// </summary>
        public const string GivenName = "given_name";

        /// <summary>
        /// Last name of the member.
        /// </summary>
        public const string FamilyName = "family_name";

        /// <summary>
        /// Email address of the member.
        /// </summary>
        public const string Email = "email";
    }
}
