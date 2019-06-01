/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

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
        public string PublicSecret { get; set; }

        public OdnoklassnikiAuthenticationOptions()
        {
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(OdnoklassnikiAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = OdnoklassnikiAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OdnoklassnikiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OdnoklassnikiAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("GET_EMAIL");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(Claims.ImageUrl, "pic_2");
        }

    }
}
