/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
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
            ClaimsIssuer = SalesforceAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SalesforceAuthenticationDefaults.CallbackPath);

            Environment = SalesforceAuthenticationDefaults.Environment;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "user_name");
            ClaimActions.MapJsonKey("urn:salesforce:email", "email");
            ClaimActions.MapJsonKey("urn:salesforce:thumbnail_photo", "thumbnail");
            ClaimActions.MapJsonKey("urn:salesforce:utc_offset", "utcOffset");
            ClaimActions.MapCustomJson("urn:salesforce:rest_url", user => user["urls"]?.Value<string>("rest"));
        }

        public SalesforceAuthenticationEnvironment Environment
        {
            set
            {
                switch (value)
                {
                    case SalesforceAuthenticationEnvironment.Production:
                        AuthorizationEndpoint = SalesforceAuthenticationDefaults.Production.AuthorizationEndpoint;
                        TokenEndpoint = SalesforceAuthenticationDefaults.Production.TokenEndpoint;
                        break;

                    case SalesforceAuthenticationEnvironment.Test:
                        AuthorizationEndpoint = SalesforceAuthenticationDefaults.Test.AuthorizationEndpoint;
                        TokenEndpoint = SalesforceAuthenticationDefaults.Test.TokenEndpoint;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported Salesforce environment");
                }
            }
        }
    }
}
