/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Snapchat;

/// <summary>
/// Contains constants specific to the <see cref="SnapchatAuthenticationHandler"/>.
/// </summary>
public static class SnapchatAuthenticationConstants
{
    public static class Claims
    {
        public const string TeamId = "organization_id";
        public const string MemberStatus = "member_status";
        public const string UserId = "id";
    }
}
