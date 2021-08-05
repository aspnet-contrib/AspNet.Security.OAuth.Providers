/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Discord
{
    /// <summary>
    /// Contains constants specific to the <see cref="DiscordAuthenticationHandler"/>.
    /// </summary>
    public static class DiscordAuthenticationConstants
    {
        public static class Claims
        {
            public const string AvatarHash = "urn:discord:avatar:hash";
            public const string Discriminator = "urn:discord:user:discriminator";
        }

        public static class UrlQueryParameterValues
        {
            public const string Consent = "consent";
            public const string None = "none";
        }
    }
}
