/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Dropbox
{
    /// <summary>
    /// Defines a set of options used by <see cref="DropboxAuthenticationHandler"/>.
    /// </summary>
    public class DropboxAuthenticationOptions : OAuthOptions
    {
        public DropboxAuthenticationOptions()
        {
            ClaimsIssuer = DropboxAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DropboxAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DropboxAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DropboxAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DropboxAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
            ClaimActions.MapJsonSubKey(ClaimTypes.Name, "name", "display_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        }
    }
}
