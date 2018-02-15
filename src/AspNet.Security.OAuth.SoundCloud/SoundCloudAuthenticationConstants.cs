/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.SoundCloud
{
    /// <summary>
    /// Contains constants specific to the <see cref="SoundCloudAuthenticationHandler"/>.
    /// </summary>
    public static class SoundCloudAuthenticationConstants
    {
        public static class Claims
        {
            public const string City = "urn:soundcloud:city";
            public const string FullName = "urn:soundcloud:fullname";
            public const string ProfileUrl = "urn:soundcloud:profileurl";
        }
    }
}
