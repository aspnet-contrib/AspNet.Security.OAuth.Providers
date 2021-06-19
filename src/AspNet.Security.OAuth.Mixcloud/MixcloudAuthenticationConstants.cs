/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Mixcloud
{
    /// <summary>
    /// Contains constants specific to the <see cref="MixcloudAuthenticationHandler"/>.
    /// </summary>
    public static class MixcloudAuthenticationConstants
    {
        public static class Claims
        {
            public const string FullName = "urn:mixcloud:fullname";
            public const string ProfileUrl = "urn:mixcloud:profileurl";
            public const string City = "urn:mixcloud:city";
            public const string Biography = "urn:mixcloud:biography";
            public const string ProfileImageUrl = "urn:mixcloud:profileimageurl";
            public const string ProfileThumbnailUrl = "urn:mixcloud:profilethumbnailurl";
        }
    }
}
