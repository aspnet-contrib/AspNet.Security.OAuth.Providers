/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Defines a set of options used by <see cref="AutodeskAuthenticationHandler"/>.
    /// </summary>
    public class AutodeskAuthenticationOptions : OAuthOptions
    {
        public AutodeskAuthenticationOptions()
        {
            AuthenticationScheme = AutodeskAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AutodeskAuthenticationDefaults.DisplayName;
            ClaimsIssuer = AutodeskAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(AutodeskAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AutodeskAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AutodeskAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AutodeskAuthenticationDefaults.UserInformationEndpoint;

            foreach (var scope in AutodeskAuthenticationDefaults.Scope)
            {
                Scope.Add(scope);
            }
        }
    }
}
