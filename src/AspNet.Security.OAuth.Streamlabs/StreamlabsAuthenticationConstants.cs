/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Streamlabs
{
    /// <summary>
    /// Contains constants specific to the <see cref="StreamlabsAuthenticationHandler"/>.
    /// </summary>
    public static class StreamlabsAuthenticationConstants
    {
        public static class Claims
        {
            public const string DisplayName = "urn:streamlabs:displayname";
            public const string FacebookId = "urn:streamlabs:facebookid";
            public const string FacebookName = "urn:streamlabs:facebookname";
            public const string Primary = "urn:streamlabs:primary";
            public const string Thumbnail = "urn:streamlabs:thumbnail";
            public const string TwitchDisplayName = "urn:streamlabs:twitchdisplayname";
            public const string TwitchIconUrl = "urn:streamlabs:twitchiconurl";
            public const string TwitchId = "urn:streamlabs:twitchid";
            public const string TwitchName = "urn:streamlabs:twitchname";
            public const string YouTubeId = "urn:streamlabs:youtubeid";
            public const string YouTubeTitle = "urn:streamlabs:youtubetitle";
        }
    }
}
