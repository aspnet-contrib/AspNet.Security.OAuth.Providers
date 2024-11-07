/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
*/

namespace AspNet.Security.OAuth.VkId;

/// <summary>
/// Contains constants specific to the <see cref="VkIdAuthenticationHandler"/>.
/// </summary>
public static class VkIdAuthenticationConstants
{
    public static class Claims
    {
        public const string Avatar = "urn:vkid:avatar:link";
        public const string IsVerified = "urn:vkid:verified";
    }

    public static class AuthenticationProperties
    {
        public const string DeviceId = "DeviceId";
    }
}
