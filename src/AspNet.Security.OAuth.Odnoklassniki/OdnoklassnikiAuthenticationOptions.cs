/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    /// <summary>
    /// Configuration options for <see cref="OdnoklassnikiAuthenticationHandler"/>.
    /// </summary>
    public class OdnoklassnikiAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="OdnoklassnikiAuthenticationOptions"/>.
        /// </summary>
        public OdnoklassnikiAuthenticationOptions()
        {
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(OdnoklassnikiAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = OdnoklassnikiAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OdnoklassnikiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OdnoklassnikiAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("VALUABLE_ACCESS");
            Scope.Add("GET_EMAIL");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey("urn:odnoklassniki:pic_1", "pic_1");
        }

        /// <summary>
        /// Gets or sets the application key used to retrieve user details from Odnoklassniki's API.
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://apiok.ru/en/dev/methods/rest/users/users.getCurrentUser for more information.
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

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(ApplicationKey))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The '{0}' option must be provided.", nameof(ApplicationKey)));
            }
        }
    }
}
