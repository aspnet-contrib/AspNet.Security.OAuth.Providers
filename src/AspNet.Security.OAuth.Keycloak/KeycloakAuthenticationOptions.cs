/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Keycloak
{
    /// <summary>
    /// Defines a set of options used by <see cref="KeycloakAuthenticationHandler"/>.
    /// </summary>
    public class KeycloakAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAuthenticationOptions"/> class.
        /// </summary>
        public KeycloakAuthenticationOptions()
        {
            ClaimsIssuer = KeycloakAuthenticationDefaults.Issuer;
            CallbackPath = KeycloakAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = KeycloakAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = KeycloakAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = KeycloakAuthenticationDefaults.UserInformationEndpoint;

            // TODO Add any required scopes
            // Scope.Add("?");

            // TODO Map any claims
            // ClaimActions.MapJsonKey(ClaimTypes.Email, "?");
        }
    }
}
