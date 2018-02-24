/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Meetup
{
    /// <summary>
    /// Contains claim types specific to the <see cref="MeetupAuthenticationHandler"/>.
    /// </summary>
    public static class MeetupClaimTypes
    {
        public const string Name = "urn:meetup:name";

        public const string Status = "urn:meetup:status";

        public const string Latitude = "urn:meetup:lat";

        public const string Longitude = "urn:meetup:lon";

        public const string Joined = "urn:meetup:joined";

        public const string City = "urn:meetup:city";

        public const string Country = "urn:meetup:country";

        public const string LocalizedCountryName = "urn:meetup:localized_country_name";

        public const string State = "urn:meetup:state";

        public const string PhotoId = "urn:meetup:photo.id";

        public const string PhotoHighResolutionLink = "urn:meetup:photo.highres_link";

        public const string PhotoLink = "urn:meetup:photo.photo_link";

        public const string PhotoThumbnailLink = "urn:meetup:photo.thumb_link";

        public const string PhotoBaseUrl = "urn:meetup:photo.base_url";

        public const string PhotoType = "urn:meetup:photo.type";

    }
}
