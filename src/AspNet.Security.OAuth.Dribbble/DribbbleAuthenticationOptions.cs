/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Dribbble
{
    /// <summary>
    /// Defines a set of options used by <see cref="DribbbleAuthenticationHandler"/>.
    /// </summary>
    public class DribbbleAuthenticationOptions : OAuthOptions
    {
        public DribbbleAuthenticationOptions()
        {
            AuthenticationScheme = DribbbleAuthenticationDefaults.AuthenticationScheme;
            DisplayName = DribbbleAuthenticationDefaults.DisplayName;
            ClaimsIssuer = DribbbleAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DribbbleAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DribbbleAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DribbbleAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DribbbleAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}