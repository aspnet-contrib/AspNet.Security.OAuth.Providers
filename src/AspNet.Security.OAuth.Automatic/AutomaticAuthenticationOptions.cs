/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Automatic
{
    /// <summary>
    /// Defines a set of options used by <see cref="AutomaticAuthenticationHandler"/>.
    /// </summary>
    public class AutomaticAuthenticationOptions : OAuthOptions
    {
        public AutomaticAuthenticationOptions()
        {
            AuthenticationScheme = AutomaticAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AutomaticAuthenticationDefaults.DisplayName;
            ClaimsIssuer = AutomaticAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(AutomaticAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AutomaticAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AutomaticAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AutomaticAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
