/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
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
            AuthenticationScheme = MailChimpAuthenticationDefaults.AuthenticationScheme;
            DisplayName = MailChimpAuthenticationDefaults.DisplayName;
            ClaimsIssuer = MailChimpAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MailChimpAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MailChimpAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MailChimpAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MailChimpAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
