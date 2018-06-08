/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Twitch
{
    /// <summary>
    /// Contains constants specific to the <see cref="TwitchAuthenticationHandler"/>.
    /// </summary>
    public static class TwitchAuthenticationConstants
    {
        public static class Claims
        {
            public const string BroadcasterType = "urn:twitch:broadcastertype";
            public const string Description = "urn:twitch:description";
            public const string DisplayName = "urn:twitch:displayname";
            public const string OfflineImageUrl = "urn:twitch:offlineimageurl";
            public const string ProfileImageUrl = "urn:twitch:profileimageurl";
            public const string Type = "urn:twitch:type";
        }
    }
}
