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
        public static class Permissions
        {
            public const string Basic_Access = "basic_access";
            public const string Email = "email";
            public const string Offline_Access = "offline_access";
            public const string Manage_Library = "manage_library";
            public const string Manage_Community = "manage_community";
            public const string Delete_Library = "delete_library";
            public const string Listening_History = "listening_history";
        }

#pragma warning restore CA1034 // Nested types should not be visible
    }
}
