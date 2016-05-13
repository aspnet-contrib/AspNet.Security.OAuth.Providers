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
        public SalesforceAuthenticationOptions OverrideHostInEndpoints(string newHost)
        {
            var authEndpointUri = new UriBuilder(AuthorizationEndpoint);
            var tokenEndpointUri = new UriBuilder(TokenEndpoint);

            authEndpointUri.Host = newHost;
            tokenEndpointUri.Host = newHost;

            AuthorizationEndpoint = authEndpointUri.ToString();
            TokenEndpoint = tokenEndpointUri.ToString();

            return this;
        }

        public SalesforceAuthenticationOptions()
        {
            AuthenticationScheme = SalesforceAuthenticationDefaults.AuthenticationScheme;
            DisplayName = SalesforceAuthenticationDefaults.DisplayName;
            ClaimsIssuer = SalesforceAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SalesforceAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SalesforceAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SalesforceAuthenticationDefaults.TokenEndpoint;
        }
    }
}
