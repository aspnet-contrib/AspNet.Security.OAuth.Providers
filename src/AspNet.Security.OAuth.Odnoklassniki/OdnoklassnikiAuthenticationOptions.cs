/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    /// <summary>
    /// Configuration options for <see cref="OdnoklassnikiAuthenticationMiddleware"/>.
    /// </summary>
    public class OdnoklassnikiAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="OdnoklassnikiAuthenticationOptions"/>.
        /// </summary>
        public OdnoklassnikiAuthenticationOptions()
        {
            AuthenticationScheme = OdnoklassnikiAuthenticationDefaults.AuthenticationScheme;
            DisplayName = OdnoklassnikiAuthenticationDefaults.DisplayName;
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(OdnoklassnikiAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = OdnoklassnikiAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OdnoklassnikiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OdnoklassnikiAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("VALUABLE_ACCESS");
            Scope.Add("GET_EMAIL");
        }

        /// <summary>
        /// Gets or sets the application key used to retrieve user details from Odnoklassniki's API.
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://apiok.ru/dev/methods/rest/users/users.getCurrentUser for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "uid",
            "name",
            "email",
            "first_name",
            "last_name",
            "pic_1"
        };
    }
}