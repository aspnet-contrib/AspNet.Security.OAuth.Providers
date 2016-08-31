/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Myob {
    /// <summary>
    /// Defines a set of options used by <see cref="MyobAuthenticationHandler"/>.
    /// </summary>
    public class MyobAuthenticationOptions : OAuthOptions {
        public MyobAuthenticationOptions() {
            AuthenticationScheme = MyobAuthenticationDefaults.AuthenticationScheme;
            DisplayName = MyobAuthenticationDefaults.DisplayName;
            ClaimsIssuer = MyobAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MyobAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MyobAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MyobAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MyobAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
