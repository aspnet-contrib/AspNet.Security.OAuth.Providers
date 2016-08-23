/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.MyobAccounting {
    /// <summary>
    /// Defines a set of options used by <see cref="MyobAccountingAuthenticationHandler"/>.
    /// </summary>
    public class MyobAccountingAuthenticationOptions : OAuthOptions {
        public MyobAccountingAuthenticationOptions() {
            AuthenticationScheme = MyobAccountingAuthenticationDefaults.AuthenticationScheme;
            DisplayName = MyobAccountingAuthenticationDefaults.DisplayName;
            ClaimsIssuer = MyobAccountingAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MyobAccountingAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MyobAccountingAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MyobAccountingAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MyobAccountingAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
