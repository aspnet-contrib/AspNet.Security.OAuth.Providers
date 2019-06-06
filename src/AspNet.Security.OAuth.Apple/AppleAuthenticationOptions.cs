/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Defines a set of options used by <see cref="AppleAuthenticationHandler"/>.
    /// </summary>
    public class AppleAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleAuthenticationOptions"/> class.
        /// </summary>
        public AppleAuthenticationOptions()
        {
            ClaimsIssuer = AppleAuthenticationDefaults.Issuer;
            CallbackPath = AppleAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = AppleAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AppleAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AppleAuthenticationDefaults.UserInformationEndpoint;

            // TODO Add any required scopes
            // Scope.Add("?");

            // TODO Map any claims
            // ClaimActions.MapJsonKey(ClaimTypes.Email, "?");
        }
    }
}
