﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// Contains claim types specific to the <see cref="PatreonAuthenticationHandler"/>.
    /// </summary>
    public static class PatreonClaimTypes
    {
        public const string Avatar = "urn:patreon:avatar";
    }
}