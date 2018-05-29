/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Yammer
{
    /// <summary>
    /// Defines a set of options used by <see cref="YammerAuthenticationHandler"/>.
    /// </summary>
    public class YammerAuthenticationOptions : OAuthOptions
    {
        public YammerAuthenticationOptions()
        {
            AuthenticationScheme = YammerAuthenticationDefaults.AuthenticationScheme;
            DisplayName = YammerAuthenticationDefaults.DisplayName;
            ClaimsIssuer = YammerAuthenticationDefaults.Issuer;

            CallbackPath = new PathString("/signin-yammer");

            AuthorizationEndpoint = YammerAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YammerAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YammerAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
