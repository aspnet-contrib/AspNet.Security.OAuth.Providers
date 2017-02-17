/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.CiscoSpark
{
    /// <summary>
    /// Defines a set of options used by <see cref="CiscoSparkAuthenticationHandler"/>.
    /// </summary>
    public class CiscoSparkAuthenticationOptions : OAuthOptions
    {
        public CiscoSparkAuthenticationOptions()
        {
            AuthenticationScheme = CiscoSparkAuthenticationDefaults.AuthenticationScheme;
            DisplayName = CiscoSparkAuthenticationDefaults.DisplayName;
            ClaimsIssuer = CiscoSparkAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(CiscoSparkAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = CiscoSparkAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = CiscoSparkAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = CiscoSparkAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}