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
    }
}
