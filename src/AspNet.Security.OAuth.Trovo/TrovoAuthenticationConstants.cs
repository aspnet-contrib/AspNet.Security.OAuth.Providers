/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Trovo;

/// <summary>
/// Contains constants specific to the <see cref="TrovoAuthenticationHandler"/>.
/// </summary>
public class TrovoAuthenticationConstants
{
    public const string GrantType = "authorization_code";

    public static class Claims
    {
        public const string ChannelId = "urn:trovo:channelid";
        public const string ProfilePic = "urn:trovo:profilepic";
    }

    public static class Headers
    {
        public const string ClientId = "client-id";
        public const string Authorization = "OAuth";
    }
}
