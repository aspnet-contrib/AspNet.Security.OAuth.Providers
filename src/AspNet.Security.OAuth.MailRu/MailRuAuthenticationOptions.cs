/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.MailRu
{
    /// <summary>
    /// Defines a set of options used by <see cref="MailRuAuthenticationHandler"/>.
    /// </summary>
    public class MailRuAuthenticationOptions : OAuthOptions
    {
        public MailRuAuthenticationOptions()
        {
            AuthenticationScheme = MailRuAuthenticationDefaults.AuthenticationScheme;
            DisplayName = MailRuAuthenticationDefaults.DisplayName;
            ClaimsIssuer = MailRuAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MailRuAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MailRuAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MailRuAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MailRuAuthenticationDefaults.UserInformationEndpoint;
        }

        //
        // Summary:
        //     Gets or sets the provider-assigned client key for signing requests.
        public string SignKey { get; set; }
    }
}