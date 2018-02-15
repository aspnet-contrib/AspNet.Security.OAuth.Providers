/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Vkontakte
{
    /// <summary>
    /// Contains constants specific to the <see cref="VkontakteAuthenticationHandler"/>.
    /// </summary>
    public static class VkontakteAuthenticationConstants
    {
        public static class Claims
        {
            public const string PhotoUrl = "urn:vkontakte:photo:link";
            public const string ThumbnailUrl = "urn:vkontakte:photo_thumb:link";
        }
    }
}
