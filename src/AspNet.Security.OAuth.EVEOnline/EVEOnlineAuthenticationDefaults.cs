﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.EVEOnline
{
    /// <summary>
    /// Default values used by the EVEOnline authentication middleware.
    /// </summary>
    public static class EVEOnlineAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "EVEOnline";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-eveonline";

        /// <summary>
        /// Default value for <see cref="EVEOnlineAuthenticationOptions.Server"/>.
        /// </summary>
        public const string TranquilityUrl = "https://login.eveonline.com";

        public const string SerenityUrl = "https://login.evepc.163.com";
    }
}
