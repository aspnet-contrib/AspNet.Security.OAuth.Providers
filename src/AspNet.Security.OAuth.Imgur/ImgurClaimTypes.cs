/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Imgur
{
    /// <summary>
    /// Contains claim types specific to the <see cref="ImgurAuthenticationHandler"/>.
    /// </summary>
    public static class ImgurClaimTypes
    {
        public const string Bio = "urn:imgur:bio";

        public const string Reputation = "urn:imgur:reputation";

        public const string Created = "urn:imgur:created";

        public const string ProExpiration = "urn:imgur:proexpiration";
    }
}