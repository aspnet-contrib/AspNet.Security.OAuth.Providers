/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zoom;

/// <summary>
/// Contains constants specific to the <see cref="ZoomAuthenticationHandler"/>.
/// </summary>
public static class ZoomAuthenticationConstants
{
    public static class Claims
    {
        public const string Picture = "picture";

        public const string Email = "email";

        public const string NameIdentifier = "id";

        public const string Name = "name";

        public const string GivenName = "given_name";

        public const string FamilyName = "family_name";

        public const string PhoneNumber = "phone_number";

        public const string Status = "account_status";

        public const string Verified = "verified";

        public const string PersonalMeetingUrl = "personal_meeting_url";
    }

    /// <summary>
    /// Available profile fields after a Zoom authentication.
    /// See <a>https://developers.zoom.us/docs/api/rest/reference/user/methods/#operation/user</a>
    /// </summary>
    public static class ProfileFields
    {
        /// <summary>
        /// The Unique identifier of the user
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// Display name of the user.
        /// </summary>
        public const string Name = "display_name";

        /// <summary>
        /// Given/First name of the user.
        /// </summary>
        public const string GivenName = "first_name";

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public const string FamilyName = "last_name";

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public const string PhoneNumber = "phone_number";

        /// <summary>
        /// Picture URL of the user.
        /// </summary>
        public const string PictureUrl = "pic_url";

        /// <summary>
        /// AccountStatus of the user.
        /// </summary>
        public const string Status = "status";

        /// <summary>
        /// Verification status of the user.
        /// </summary>
        public const string Verified = "verified";

        /// <summary>
        /// Personal meeting URL of the user.
        /// </summary>
        public const string PersonalMeetingUrl = "personal_meeting_url";
    }
}
