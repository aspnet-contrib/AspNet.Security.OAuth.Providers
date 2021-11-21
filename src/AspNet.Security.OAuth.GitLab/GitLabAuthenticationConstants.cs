/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitLab;

/// <summary>
/// Contains constants specific to the <see cref="GitLabAuthenticationHandler"/>.
/// </summary>
public static class GitLabAuthenticationConstants
{
    public static class Claims
    {
        public const string Name = "urn:gitlab:name";
        public const string Avatar = "urn:gitlab:avatar";
        public const string Url = "urn:gitlab:url";
    }
}
