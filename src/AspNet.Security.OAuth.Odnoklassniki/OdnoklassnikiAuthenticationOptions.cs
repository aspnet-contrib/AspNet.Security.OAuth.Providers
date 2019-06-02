/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Odnoklassniki.OdnoklassnikiAuthenticationConstants;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    /// <summary>
    /// Defines a set of options used by <see cref="OdnoklassnikiAuthenticationHandler"/>.
    /// App registration could be performed by using next instruction: https://apiok.ru/dev/app/create.
    /// After registration you will receive email with 3 parameters:
    /// 1. Application ID - mapped to ClientId
    /// 2. Public App key - mapped to PublicSecret
    /// 3. Secret App key - mapped to ClientSecret
    /// Email retrieval requires request to provider on api-support@ok.ru for GET_EMAIL permission (https://apiok.ru/ext/oauth/permissions).
    /// </summary>
    public class OdnoklassnikiAuthenticationOptions : OAuthOptions
    {
        public OdnoklassnikiAuthenticationOptions()
        {
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;

            CallbackPath = OdnoklassnikiAuthenticationDefaults.CallbackPath;

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
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
            ClaimActions.MapJsonKey(Claims.Pic1, "pic_1");
            ClaimActions.MapJsonKey(Claims.Pic2, "pic_2");
            ClaimActions.MapJsonKey(Claims.Pic3, "pic_3");
        }

        /// <summary>
        /// Public App Key from application registration email.
        /// </summary>
        public string PublicSecret { get; set; }
    }
}
