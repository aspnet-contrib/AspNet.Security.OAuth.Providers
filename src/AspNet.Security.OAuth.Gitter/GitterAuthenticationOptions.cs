/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Gitter
{
    public class GitterAuthenticationOptions : OAuthOptions
    {
        public GitterAuthenticationOptions()
        {
            AuthenticationScheme = GitterAuthenticationDefaults.AuthenticationScheme;
            DisplayName = GitterAuthenticationDefaults.DisplayName;
            ClaimsIssuer = GitterAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitterAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitterAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitterAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitterAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}