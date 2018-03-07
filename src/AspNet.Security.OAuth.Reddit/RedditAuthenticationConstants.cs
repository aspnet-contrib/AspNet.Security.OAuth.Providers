/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Reddit
{
    /// <summary>
    /// Contains constants specific to the <see cref="RedditAuthenticationHandler"/>.
    /// </summary>
    public static class RedditAuthenticationConstants
    {
        public static class Claims
        {
            public const string Over18 = "urn:reddit:over18";
        }
    }
}
