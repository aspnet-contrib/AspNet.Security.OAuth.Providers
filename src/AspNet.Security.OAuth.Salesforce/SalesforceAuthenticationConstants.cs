/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Contains constants specific to the <see cref="SalesforceAuthenticationHandler"/>.
    /// </summary>
    public static class SalesforceAuthenticationConstants
    {
        public static class Claims
        {
            public const string Email = "urn:salesforce:email";
            public const string RestUrl = "urn:salesforce:rest_url";
            public const string ThumbnailPhoto = "urn:salesforce:thumbnail_photo";
            public const string UtcOffset = "urn:salesforce:utc_offset";
        }
    }
}
