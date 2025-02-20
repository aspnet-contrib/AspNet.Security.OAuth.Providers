/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitCode;

/// <summary>
/// Contains constants specific to the <see cref="GitCodeAuthenticationHandler"/>.
/// </summary>
public static class GitCodeAuthenticationConstants
{
    public static class Claims
    {
        public const string AvatarUrl = "urn:gitcode:avatar_url";
        public const string Bio = "urn:gitcode:bio";
        public const string Blog = "urn:gitcode:blog";
        public const string Company = "urn:gitcode:company";
        public const string HtmlUrl = "urn:gitcode:html_url";
        public const string Name = "urn:gitcode:name";
    }
}
