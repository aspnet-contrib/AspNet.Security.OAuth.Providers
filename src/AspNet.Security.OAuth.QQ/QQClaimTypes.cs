/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.QQ
{
    /// <summary>
    /// Contains claim types specific to the <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public static class QQClaimTypes
    {
        public const string PictureUrl = "urn:qq:picture";

        public const string PictureMediumUrl = "urn:qq:picture_medium";

        public const string PictureFullUrl = "urn:qq:picture_full";

        public const string AvatarUrl = "urn:qq:avatar";

        public const string AvatarFullUrl = "urn:qq:avatar_full";
    }
}
