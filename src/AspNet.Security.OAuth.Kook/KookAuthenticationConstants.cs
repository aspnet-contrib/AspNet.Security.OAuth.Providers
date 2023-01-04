/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Kook;

/// <summary>
/// Contains constants specific to the <see cref="KookAuthenticationHandler"/>.
/// </summary>
public static class KookAuthenticationConstants
{
    public static class Claims
    {
        public const string IsOnline = "urn:kook:user:is_online";
        public const string IsBanned = "urn:kook:user:is_banned";
        public const string OperatingSystem = "urn:kook:user:operating_system";
        public const string AvatarUrl = "urn:kook:user:avatar:url";
        public const string BannerUrl = "urn:kook:user:banner:url";
        public const string IsBot = "urn:kook:user:is_bot";
        public const string IsMobileVerified = "urn:kook:user:is_mobile_verified";
        public const string IdentifyNumber = "urn:kook:user:identify_num";
    }
}
