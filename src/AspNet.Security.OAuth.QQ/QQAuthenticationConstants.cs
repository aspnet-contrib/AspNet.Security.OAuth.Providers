/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.QQ
{
    /// <summary>
    /// Contains constants specific to the <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public static class QQAuthenticationConstants
    {
        public static class Claims
        {
            public const string AvatarFullUrl = "urn:qq:avatar_full";
            public const string AvatarUrl = "urn:qq:avatar";
            public const string PictureFullUrl = "urn:qq:picture_full";
            public const string PictureMediumUrl = "urn:qq:picture_medium";
            public const string PictureUrl = "urn:qq:picture";
        }
    }
}
