/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WordPress
{
    /// <summary>
    /// Contains constants specific to the <see cref="WordPressAuthenticationHandler"/>.
    /// </summary>
    public static class WordPressAuthenticationConstants
    {
        public static class Claims
        {
            public const string AvatarUrl = "urn:wordpress:avatarurl";
            public const string Email = "urn:wordpress:email";
            public const string DisplayName = "urn:wordpress:displayname";
            public const string PrimaryBlog = "urn:wordpress:primaryblog";
            public const string ProfileUrl = "urn:wordpress:profileurl";
        }
    }
}
