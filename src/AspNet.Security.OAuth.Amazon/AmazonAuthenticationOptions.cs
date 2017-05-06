/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
            AuthenticationScheme = AmazonAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AmazonAuthenticationDefaults.DisplayName;
            ClaimsIssuer = AmazonAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(AmazonAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AmazonAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AmazonAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AmazonAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("profile");
            Scope.Add("profile:user_id");

            Fields.Add("email");
            Fields.Add("name");
            Fields.Add("user_id");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}
