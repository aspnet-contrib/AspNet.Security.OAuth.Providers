/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Amazon
{
    /// <summary>
    /// Defines a set of options used by <see cref="AmazonAuthenticationHandler"/>.
    /// </summary>
    public class AmazonAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AmazonAuthenticationOptions"/> class.
        /// </summary>
        public AmazonAuthenticationOptions()
        {
            ClaimsIssuer = AmazonAuthenticationDefaults.Issuer;

            CallbackPath = AmazonAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = AmazonAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AmazonAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AmazonAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("profile");
            Scope.Add("profile:user_id");

            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.PostalCode, "postal_code");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "email",
            "name",
            "user_id"
        };
    }
}
