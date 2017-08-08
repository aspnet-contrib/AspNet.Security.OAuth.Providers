/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Intuit
{
    /// <summary>
    /// Default values used by the Intuit authentication middleware.
    /// </summary>
    public static class IntuitAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "Intuit";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Intuit";

        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Intuit";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/qbauth/callback";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://appcenter.intuit.com/connect/oauth2";

        public const string UserInformationEndpoint = "https://quickbooks.api.intuit.com/v3/company/";
        public static ICollection<string> Scope { get; set; }
        public static string TokenEndpoint = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
    }
}