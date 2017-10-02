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
            ClaimActions.MapJsonKey("urn:linkedin:maidenname", "maidenName");
            ClaimActions.MapJsonKey("urn:linkedin:profile", "publicProfileUrl");
            ClaimActions.MapJsonKey("urn:linkedin:profilepicture", "pictureUrl");
            ClaimActions.MapJsonKey("urn:linkedin:industry", "industry");
            ClaimActions.MapJsonKey("urn:linkedin:summary", "summary");
            ClaimActions.MapJsonKey("urn:linkedin:headline", "headline");
            ClaimActions.MapCustomJson("urn:linkedin:positions", user => user["positions"]?.ToString());
            ClaimActions.MapJsonKey("urn:linkedin:phoneticfirstname", "phoneticFirstName");
            ClaimActions.MapJsonKey("urn:linkedin:phoneticlastname", "phoneticLastName");
            ClaimActions.MapJsonKey("urn:linkedin:phoneticname", "formattedPhoneticName");
            ClaimActions.MapCustomJson("urn:linkedin:location", user => user["location"]?.ToString());
            ClaimActions.MapJsonKey("urn:linkedin:specialties", "specialties");
            ClaimActions.MapJsonKey("urn:linkedin:numconnections", "numConnections");
            ClaimActions.MapJsonKey("urn:linkedin:numconnectionscapped", "numConnectionsCapped");
            ClaimActions.MapJsonKey("urn:linkedin:currentshare", "currentShare");
            ClaimActions.MapCustomJson("urn:linkedin:pictureurls", user => user["pictureUrls"]?.ToString());
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