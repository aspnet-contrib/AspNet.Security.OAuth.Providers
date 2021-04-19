/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WorkWeixin
{
    /// <summary>
    /// Contains constants specific to the <see cref="WorkWeixinAuthenticationHandler"/>.
    /// </summary>
    public static class WorkWeixinAuthenticationConstants
    {
        public static class Claims
        {
            public const string Avatar = "urn:workweixin:avatar";
            public const string Mobile = "urn:workweixin:mobile";
            public const string Alias = "urn:workweixin:alias";
        }
    }
}
