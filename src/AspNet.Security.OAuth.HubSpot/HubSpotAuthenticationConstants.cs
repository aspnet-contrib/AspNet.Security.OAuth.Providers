/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.HubSpot
{
    /// <summary>
    /// Contains constants specific to the <see cref="HubSpotAuthenticationHandler"/>.
    /// </summary>
    public static class HubSpotAuthenticationConstants
    {
        public static class Claims
        {
            public const string AppId = "urn:hubspot:app_id";
            public const string Email = "urn:hubspot:email";
            public const string HubDomain = "urn:hubspot:hub_domain";
            public const string HubId = "urn:hubspot:hub_id";
            public const string UserId = "urn:hubspot:user_id";
        }
    }
}
