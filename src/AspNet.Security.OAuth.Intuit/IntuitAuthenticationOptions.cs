/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Intuit
{
    public class IntuitAuthenticationOptions : OAuthOptions
    {
        public IntuitAuthenticationOptions()
        {
            AuthenticationScheme = IntuitAuthenticationDefaults.AuthenticationScheme;
            UserInformationEndpoint = IntuitAuthenticationDefaults.UserInformationEndpoint;
            DisplayName = IntuitAuthenticationDefaults.DisplayName;
            ClaimsIssuer = IntuitAuthenticationDefaults.Issuer;
            SignInScheme = "Intuit";
            CallbackPath = new PathString(IntuitAuthenticationDefaults.CallbackPath);
           
            AuthorizationEndpoint = IntuitAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = IntuitAuthenticationDefaults.TokenEndpoint;
            Scope.Add("com.intuit.quickbooks.accounting");
        }
    }
}