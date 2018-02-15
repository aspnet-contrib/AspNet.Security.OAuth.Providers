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
using static AspNet.Security.OAuth.LinkedIn.LinkedInAuthenticationConstants;

namespace AspNet.Security.OAuth.LinkedIn
{
    /// <summary>
    /// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public class LinkedInAuthenticationOptions : OAuthOptions
    {
        public LinkedInAuthenticationOptions()
        {
            ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(LinkedInAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "emailAddress");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "formattedName");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
            ClaimActions.MapJsonKey(Claims.MaidenName, "maidenName");
            ClaimActions.MapJsonKey(Claims.ProfileUrl, "publicProfileUrl");
            ClaimActions.MapJsonKey(Claims.PictureUrl, "pictureUrl");
            ClaimActions.MapJsonKey(Claims.Industry, "industry");
            ClaimActions.MapJsonKey(Claims.Summary, "summary");
            ClaimActions.MapJsonKey(Claims.Headline, "headline");
            ClaimActions.MapCustomJson(Claims.Positions, user => user["positions"]?.ToString());
            ClaimActions.MapJsonKey(Claims.PhoneticFirstName, "phoneticFirstName");
            ClaimActions.MapJsonKey(Claims.PhoneticLastName, "phoneticLastName");
            ClaimActions.MapJsonKey(Claims.FormattedPhoneticName, "formattedPhoneticName");
            ClaimActions.MapCustomJson(Claims.Location, user => user["location"]?.ToString());
            ClaimActions.MapJsonKey(Claims.Specialties, "specialties");
            ClaimActions.MapJsonKey(Claims.NumConnections, "numConnections");
            ClaimActions.MapJsonKey(Claims.NumConnectionsCapped, "numConnectionsCapped");
            ClaimActions.MapJsonKey(Claims.CurrentShare, "currentShare");
            ClaimActions.MapCustomJson(Claims.PictureUrls, user => user["pictureUrls"]?.ToString());
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://developer.linkedin.com/docs/fields/basic-profile for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "id",
            "first-name",
            "last-name",
            "formatted-name",
            "email-address"
        };
    }
}
