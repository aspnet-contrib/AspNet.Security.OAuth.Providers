/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitHub
{
    /// <summary>
    /// Contains constants specific to the <see cref="GitHubAuthenticationHandler"/>.
    /// </summary>
    public static class GitHubAuthenticationConstants
    {
        public static class Claims
        {
            public const string Name = "urn:github:name";
            public const string Url = "urn:github:url";
        }
    }
}
