/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Nextcloud
{
    /// <summary>
    /// Contains constants specific to the <see cref="NextcloudAuthenticationHandler"/>.
    /// </summary>
    public static class NextcloudAuthenticationConstants
    {
        public static class Claims
        {
            public const string Groups = "urn:nextcloud:groups";
            public const string DisplayName = "urn:nextcloud:displayname";
            public const string IsEnabled = "urn:nextcloud:enabled";
            public const string Language = "urn:nextcloud:language";
            public const string Locale = "urn:nextcloud:locale";
        }
    }
}
