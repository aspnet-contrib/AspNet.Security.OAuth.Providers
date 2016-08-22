/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * See 
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.MYOB {
    /// <summary>
    /// Defines a set of options used by <see cref="MYOBAuthenticationHandler"/>.
    /// </summary>
    public class MYOBAuthenticationOptions : OAuthOptions {
        public MYOBAuthenticationOptions() {
            AuthenticationScheme = MYOBAuthenticationDefaults.AuthenticationScheme;
            DisplayName = MYOBAuthenticationDefaults.DisplayName;
            ClaimsIssuer = MYOBAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MYOBAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MYOBAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MYOBAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MYOBAuthenticationDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; } = MYOBAuthenticationDefaults.UserEmailsEndpoint;
    }
}
