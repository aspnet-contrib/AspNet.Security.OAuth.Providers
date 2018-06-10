using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AspNet.Security.OAuth.Twitch
{
    /// <summary>
    /// Contains constants specific to the <see cref="TwitchAuthenticationHandler"/>.
    /// </summary>
    public static class TwitchAuthenticationConstants
    {
        public static class Claims
        {
            public const string DisplayName = "urn:twitch:displayname";
            public const string Type = "urn:twitch:type";
            public const string BroadcasterType = "urn:twitch:broadcastertype";
            public const string Description = "urn:twitch:description";
            public const string ProfileImageUrl = "urn:twitch:profileimageurl";
            public const string OfflineImageUrl = "urn:twitch:offlineimageurl";
        }
    }
}
