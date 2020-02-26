/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Salesforce.SalesforceAuthenticationConstants;

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

            CallbackPath = SalesforceAuthenticationDefaults.CallbackPath;

            Environment = SalesforceAuthenticationDefaults.Environment;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(Claims.Email, "email");
            ClaimActions.MapJsonKey(Claims.UtcOffset, "utcOffset");
            ClaimActions.MapJsonSubKey(Claims.RestUrl, "urls", "rest");
            ClaimActions.MapJsonSubKey(Claims.ThumbnailPhoto, "photos", "thumbnail");
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
