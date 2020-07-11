/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Instagram
{
    /// <summary>
    /// Contains constants specific to the <see cref="InstagramAuthenticationHandler"/>.
    /// </summary>
    public static class InstagramAuthenticationConstants
    {
        /// <summary>
        /// Contains constants for claims.
        /// </summary>
        public static class Claims
        {
            /// <summary>
            /// The claim for the user's account type.
            /// </summary>
            public const string AccountType = "urn:instagram:accounttype";

            /// <summary>
            /// The claim for the user's media count.
            /// </summary>
            public const string MediaCount = "urn:instagram:mediacount";
        }
    }
}
