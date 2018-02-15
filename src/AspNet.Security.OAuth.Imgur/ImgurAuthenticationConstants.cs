/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Imgur
{
    /// <summary>
    /// Contains constants specific to the <see cref="ImgurAuthenticationHandler"/>.
    /// </summary>
    public static class ImgurAuthenticationConstants
    {
        public static class Claims
        {
            public const string Bio = "urn:imgur:bio";
            public const string Created = "urn:imgur:created";
            public const string ProExpiration = "urn:imgur:proexpiration";
            public const string Reputation = "urn:imgur:reputation";
        }
    }
}
