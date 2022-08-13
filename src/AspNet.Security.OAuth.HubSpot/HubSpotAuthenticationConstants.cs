/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.HubSpot;

/// <summary>
/// Contains constants specific to the <see cref="HubSpotAuthenticationHandler"/>.
/// </summary>
public static class HubSpotAuthenticationConstants
{
    public static class Claims
    {
        public const string HubId = "urn:HubSpot:hub_id";
        public const string UserId = "urn:HubSpot:user_id";
        public const string AppId = "urn:HubSpot:app_id";
        public const string HubDomain = "urn:HubSpot:hub_domain";
    }
}
