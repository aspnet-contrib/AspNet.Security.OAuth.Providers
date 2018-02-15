/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Spotify
{
    /// <summary>
    /// Contains constants specific to the <see cref="SpotifyAuthenticationOptions"/>.
    /// </summary>
    public static class SpotifyAuthenticationConstants
    {
        public static class Claims
        {
            public const string Product = "urn:spotify:product";
            public const string ProfilePicture = "urn:spotify:profilepicture";
            public const string Url = "urn:spotify:url";
        }
    }
}
