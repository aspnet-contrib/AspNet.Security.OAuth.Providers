/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Yammer
{
    /// <summary>
    /// Contains constants specific to the <see cref="YammerAuthenticationHandler"/>.
    /// </summary>
    public static class YammerAuthenticationConstants
    {
        public static class Claims
        {
            public const string JobTitle = "urn:yammer:job_title";
            public const string WebUrl = "urn:yammer:link";
        }
    }
}
