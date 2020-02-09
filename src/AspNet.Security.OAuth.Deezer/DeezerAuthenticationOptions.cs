/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Deezer.DeezerAuthenticationConstants;

namespace AspNet.Security.OAuth.Deezer
{
    /// <summary>
    /// Defines a set of options used by <see cref="DeezerAuthenticationHandler"/>.
    /// </summary>
    public class DeezerAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Deezer API Permissions
        /// <para>https://developers.deezer.com/api/permissions</para>
        /// </summary>
        public DeezerAuthenticationOptions()
        {
            ClaimsIssuer = DeezerAuthenticationDefaults.Issuer;

            CallbackPath = DeezerAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = DeezerAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DeezerAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DeezerAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstname");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastname");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");

            ClaimActions.MapJsonKey(Claims.Username, "name");
            ClaimActions.MapJsonKey(Claims.Avatar, "picture");
            ClaimActions.MapJsonKey(Claims.AvatarXL, "picture_xl");
            ClaimActions.MapJsonKey(Claims.AvatarBig, "picture_big");
            ClaimActions.MapJsonKey(Claims.AvatarMedium, "picture_medium");
            ClaimActions.MapJsonKey(Claims.AvatarSmall, "picture_small");
            ClaimActions.MapJsonKey(Claims.Url, "link");
            ClaimActions.MapJsonKey(Claims.Status, "status");
            ClaimActions.MapJsonKey(Claims.InscriptionDate, "inscription_date");
            ClaimActions.MapJsonKey(Claims.Language, "lang");
            ClaimActions.MapJsonKey(Claims.IsKid, "is_kid");
            ClaimActions.MapJsonKey(Claims.Tracklist, "tracklist");
            ClaimActions.MapJsonKey(Claims.Type, "type");
            ClaimActions.MapJsonKey(Claims.ExplicitContentLevel, "explicit_content_level");

            Scope.Add(Scopes.Identity);
        }
    }
}
