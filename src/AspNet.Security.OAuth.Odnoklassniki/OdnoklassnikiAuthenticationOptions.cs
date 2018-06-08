/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Odnoklassniki.OdnoklassnikiAuthenticationConstants;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    /// <summary>
    /// Defines a set of options used by <see cref="OdnoklassnikiAuthenticationHandler"/>.
    /// </summary>
    public class OdnoklassnikiAuthenticationOptions : OAuthOptions
    {
        public OdnoklassnikiAuthenticationOptions()
        {
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(OdnoklassnikiAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = OdnoklassnikiAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OdnoklassnikiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OdnoklassnikiAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("VALUABLE_ACCESS");
            Scope.Add("LONG_ACCESS_TOKEN");
            Scope.Add("GET_EMAIL");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey(Claims.Pic1, "pic_1");
            ClaimActions.MapJsonKey(Claims.Pic2, "pic_2");
            ClaimActions.MapJsonKey(Claims.Pic3, "pic_3");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://apiok.ru/dev/methods/rest/users/users.getCurrentUser for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "uid",
            "email",
            "first_name",
            "last_name",
            "gender",
            "birthday",
            "pic_1",
            "pic_2",
            "pic_3"
        } as ISet<string>;

        /// <summary> 
        /// Gets or sets the provider-assigned application key.
        /// </summary>
        public string ApplicationKey { get; set; }
    }
}
