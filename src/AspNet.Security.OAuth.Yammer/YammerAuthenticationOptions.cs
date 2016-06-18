/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.Yammer;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Configuration options for <see cref="YammerAuthenticationMiddleware"/>.
    /// </summary>
    public class YammerAuthenticationOptions : OAuthOptions {
        /// <summary>
        /// Initializes a new <see cref="YammerAuthenticationOptions"/>.
        /// </summary>
        public YammerAuthenticationOptions() {
            AuthenticationScheme = YammerAuthenticationDefaults.AuthenticationScheme;
            DisplayName = YammerAuthenticationDefaults.DisplayName;
            CallbackPath = new PathString("/signin-yammer");
            AuthorizationEndpoint = YammerAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YammerAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YammerAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}