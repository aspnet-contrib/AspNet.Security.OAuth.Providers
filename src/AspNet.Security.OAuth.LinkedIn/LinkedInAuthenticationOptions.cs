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
            ClaimActions.MapJsonKey(LinkedInClaimTypes.MaidenName, "maidenName");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.ProfileUrl, "publicProfileUrl");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.PictureUrl, "pictureUrl");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.Industry, "industry");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.Summary, "summary");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.Headline, "headline");
            ClaimActions.MapCustomJson(LinkedInClaimTypes.Positions, user => user["positions"]?.ToString());
            ClaimActions.MapJsonKey(LinkedInClaimTypes.PhoneticFirstName, "phoneticFirstName");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.PhoneticLastName, "phoneticLastName");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.FormattedPhoneticName, "formattedPhoneticName");
            ClaimActions.MapCustomJson(LinkedInClaimTypes.Location, user => user["location"]?.ToString());
            ClaimActions.MapJsonKey(LinkedInClaimTypes.Specialties, "specialties");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.NumConnections, "numConnections");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.NumConnectionsCapped, "numConnectionsCapped");
            ClaimActions.MapJsonKey(LinkedInClaimTypes.CurrentShare, "currentShare");
            ClaimActions.MapCustomJson(LinkedInClaimTypes.PictureUrls, user => user["pictureUrls"]?.ToString());
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