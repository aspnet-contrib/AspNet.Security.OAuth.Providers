/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.Spotify.SpotifyAuthenticationConstants;

namespace AspNet.Security.OAuth.Spotify
{
    /// <summary>
    /// Defines a set of options used by <see cref="SpotifyAuthenticationHandler"/>.
    /// </summary>
    public class SpotifyAuthenticationOptions : OAuthOptions
    {
        public SpotifyAuthenticationOptions()
        {
            ClaimsIssuer = SpotifyAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SpotifyAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SpotifyAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SpotifyAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = SpotifyAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthdate");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(ClaimTypes.Uri, "uri");
            ClaimActions.MapJsonKey(Claims.Product, "product");
            ClaimActions.MapJsonSubKey(Claims.Url, "external_urls", "spotify");
            ClaimActions.MapCustomJson(Claims.ProfilePicture, user => user.Value<JArray>("images")?.First?.Value<string>("url"));
        }
    }
}
