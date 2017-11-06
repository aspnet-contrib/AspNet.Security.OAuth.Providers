/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.MailChimp
{
    /// <summary>
    /// Defines a set of options used by <see cref="MailChimpAuthenticationHandler"/>.
    /// </summary>
    public class MailChimpAuthenticationOptions : OAuthOptions
    {
        public MailChimpAuthenticationOptions()
        {
            ClaimsIssuer = MailChimpAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MailChimpAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MailChimpAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MailChimpAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MailChimpAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "login", "login_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "accountname");
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "login", "login_email");
        }
    }
}
