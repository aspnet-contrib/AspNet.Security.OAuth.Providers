/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Defines a set of options used by <see cref="SalesforceAuthenticationHandler"/>.
    /// </summary>
    public class SalesforceAuthenticationOptions : OAuthOptions
    {
        public SalesforceAuthenticationOptions()
        {
            AuthenticationScheme = SalesforceAuthenticationDefaults.AuthenticationScheme;
            DisplayName = SalesforceAuthenticationDefaults.DisplayName;
            ClaimsIssuer = SalesforceAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SalesforceAuthenticationDefaults.CallbackPath);

            Environment = SalesforceAuthenticationDefaults.Environment;
        }

        public SalesforceEnvironment Environment
        {
            set
            {
                switch (value)
                {
                    case SalesforceEnvironment.Production:
                        AuthorizationEndpoint = SalesforceAuthenticationDefaults.ProductionEnvironment.AuthorizationEndpoint;
                        TokenEndpoint = SalesforceAuthenticationDefaults.ProductionEnvironment.TokenEndpoint;
                        break;
                    case SalesforceEnvironment.Test:
                        AuthorizationEndpoint = SalesforceAuthenticationDefaults.TestEnvironment.AuthorizationEndpoint;
                        TokenEndpoint = SalesforceAuthenticationDefaults.TestEnvironment.TokenEndpoint;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported Salesforce environment");
                }
            }
        }
    }
}
