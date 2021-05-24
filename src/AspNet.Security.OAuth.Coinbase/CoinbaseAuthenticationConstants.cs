/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Coinbase
{
    /// <summary>
    /// Contains constants specific to the <see cref="CoinbaseAuthenticationHandler"/>.
    /// </summary>
    public static class CoinbaseAuthenticationConstants
    {
        public static class Claims
        {
            public const string Username = "urn:coinbase:username";
            public const string ProfileLocation = "urn:coinbase:profile_location";
            public const string ProfileBio = "urn:coinbase:profile_bio";
            public const string ProfileUrl = "urn:coinbase:profile_url";
            public const string AvatarUrl = "urn:coinbase:avatar_url";
        }
    }
}
