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
        public List<string> Permissions { get; set; } = new List<string> { DeezerAuthenticationConstants.Permissions.Basic_Access };

        public DeezerAuthenticationOptions()
        {
            ClaimsIssuer = DeezerAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DeezerAuthenticationDefaults.CallbackPath);

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
            ClaimActions.MapJsonKey(Claims.Avatar_XL, "picture_xl");
            ClaimActions.MapJsonKey(Claims.Avatar_Big, "picture_big");
            ClaimActions.MapJsonKey(Claims.Avatar_Medium, "picture_medium");
            ClaimActions.MapJsonKey(Claims.Avatar_Small, "picture_small");
            ClaimActions.MapJsonKey(Claims.Url, "link");
            ClaimActions.MapJsonKey(Claims.Status, "status");
            ClaimActions.MapJsonKey(Claims.Inscription_Date, "inscription_date");
            ClaimActions.MapJsonKey(Claims.Language, "lang");
            ClaimActions.MapJsonKey(Claims.IsKid, "is_kid");
            ClaimActions.MapJsonKey(Claims.Tracklist, "tracklist");
            ClaimActions.MapJsonKey(Claims.Type, "type");
            ClaimActions.MapJsonKey(Claims.Explicit_Content_Level, "explicit_content_level");
        }

        //
        // Summary:
        //     Gets or sets the provider-assigned app id.
        public string App_Id
        {
            get => ClientId;
            set => ClientId = value;
        }

        //
        // Summary:
        //     Gets or sets the provider-assigned app secret.
        public string Secret
        {
            get => ClientSecret;
            set => ClientSecret = value;
        }

        public override void Validate()
        {
            base.Validate();
        }
    }
}
