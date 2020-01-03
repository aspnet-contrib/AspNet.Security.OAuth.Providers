/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Deezer
{
    /// <summary>
    /// Contains constants specific to the <see cref="DeezerAuthenticationHandler"/>.
    /// </summary>
    public static class DeezerAuthenticationConstants
    {
#pragma warning disable CA1034 // Nested types should not be visible

        public static class Claims
        {
            public const string Username = "urn:deezer:username";
            public const string Avatar = "urn:deezer:picture";
            public const string Avatar_XL = "urn:deezer:picture_xl";
            public const string Avatar_Big = "urn:deezer:picture_big";
            public const string Avatar_Medium = "urn:deezer:picture_medium";
            public const string Avatar_Small = "urn:deezer:picture_small";
            public const string Url = "urn:deezer:link";
            public const string Status = "urn:deezer:status";
            public const string Inscription_Date = "urn:deezer:inscription_date";
            public const string Language = "urn:deezer:lang";
            public const string IsKid = "urn:deezer:is_kid";
            public const string Tracklist = "urn:deezer:tracklist";
            public const string Type = "urn:deezer:type";
            public const string Explicit_Content_Level = "urn:deezer:explicit_content_level";
        }

        /// <summary>
        /// Deezer API Permissions
        /// <para>https://developers.deezer.com/api/permissions</para>
        /// </summary>
        public static class Scopes
        {
            /// <summary>
            /// Access users basic information
            /// <para>Incl. name, firstname, profile picture only.</para>
            /// <para>Dafault permission</para>
            /// </summary>
            public const string Identity = "basic_access";

            /// <summary>
            /// Get the user's email
            /// </summary>
            public const string Email = "email";

            /// <summary>
            /// Access user data any time
            /// <para>Application may access user data at any time.</para>
            /// </summary>
            public const string Offline_Access = "offline_access";

            /// <summary>
            /// Manage users' library
            /// <para>Add/rename a playlist. Add/order songs in the playlist.</para>
            /// </summary>
            public const string Manage_Library = "manage_library";

            /// <summary>
            /// Manage users' friends
            /// <para>Add/remove a following/follower.</para>
            /// </summary>
            public const string Manage_Community = "manage_community";

            /// <summary>
            /// Delete library itemse
            /// <para>Allow the application to delete items in the user's library.</para>
            /// </summary>
            public const string Delete_Library = "delete_library";

            /// <summary>
            /// Allow the application to access the user's listening history
            /// </summary>
            public const string Listening_History = "listening_history";
        }

#pragma warning restore CA1034 // Nested types should not be visible
    }
}
