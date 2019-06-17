/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Trakt
{
    /// <summary>
    /// Contains constants specific to the <see cref="TraktAuthenticationHandler"/>.
    /// </summary>
    public static class TraktAuthenticationConstants
    {
        public static class Claims
        {
            public const string Vip = "urn:trakt:vip";
            public const string VipEp = "urn:trakt:vip_ep";
            public const string Private = "urn:trakt:private";
        }
    }
}
