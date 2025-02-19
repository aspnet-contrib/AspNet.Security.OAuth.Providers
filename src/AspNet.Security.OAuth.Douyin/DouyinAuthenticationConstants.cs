/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Douyin;

/// <summary>
/// Contains constants specific to the <see cref="DouyinAuthenticationHandler"/>.
/// </summary>
public static class DouyinAuthenticationConstants
{
    public static class Claims
    {
        public const string Avatar = "urn:douyin:avatar";

        public const string Nickname = "urn:douyin:nickname";
    }
}
