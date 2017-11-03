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

namespace AspNet.Security.OAuth.Foursquare
{
    /// <summary>
    /// Defines a set of options used by <see cref="FoursquareAuthenticationHandler"/>.
    /// </summary>
    public class FoursquareAuthenticationOptions : OAuthOptions
    {
        public FoursquareAuthenticationOptions()
        {
            ClaimsIssuer = FoursquareAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(FoursquareAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FoursquareAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FoursquareAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FoursquareAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Uri, "canonicalUrl");
            ClaimActions.MapCustomJson(ClaimTypes.Name, user => $"{user.Value<string>("firstName")} {user.Value<string>("lastName")}");
            ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.Value<JObject>("contact")?.Value<string>("email"));
        }

        /// <summary>
        /// Gets or sets the API version used when communicating with Foursquare.
        /// See https://developer.foursquare.com/overview/versioning for more information.
        /// </summary>
        public string ApiVersion { get; set; } = FoursquareAuthenticationDefaults.ApiVersion;
    }
}
