/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Bitbucket
{
    /// <summary>
    /// Contains constants specific to the <see cref="BitbucketAuthenticationHandler"/>.
    /// </summary>
    public static class BitbucketAuthenticationConstants
    {
        public static class Claims
        {
            public const string DisplayName = "urn:bitbucket:name";
            public const string Website = "urn:bitbucket:url";
        }
    }
}
