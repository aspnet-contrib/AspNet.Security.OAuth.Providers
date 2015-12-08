/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Dropbox {
    /// <summary>
    /// Defines a set of options used by <see cref="DropboxAuthenticationHandler"/>.
    /// </summary>
    public class DropboxAuthenticationOptions : OAuthOptions {
        public DropboxAuthenticationOptions() {
            AuthenticationScheme = DropboxAuthenticationDefaults.AuthenticationScheme;
            DisplayName = DropboxAuthenticationDefaults.DisplayName;
            ClaimsIssuer = DropboxAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DropboxAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DropboxAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DropboxAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DropboxAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
