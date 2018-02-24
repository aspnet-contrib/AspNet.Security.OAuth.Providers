/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Meetup
{
    /// <summary>
    /// Defines a set of options used by <see cref="MeetupAuthenticationHandler"/>.
    /// </summary>
    public class MeetupAuthenticationOptions : OAuthOptions
    {
        public MeetupAuthenticationOptions()
        {
            ClaimsIssuer = MeetupAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(MeetupAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MeetupAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MeetupAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MeetupAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

			// Map custom claims
            ClaimActions.MapJsonKey("urn:meetup:name", "name");
            ClaimActions.MapJsonKey("urn:meetup:status", "status");
            ClaimActions.MapJsonKey("urn:meetup:lat", "lat");
            ClaimActions.MapJsonKey("urn:meetup:lon", "lon");
            ClaimActions.MapJsonKey("urn:meetup:joined", "joined");
            ClaimActions.MapJsonKey("urn:meetup:city", "city");
            ClaimActions.MapJsonKey("urn:meetup:country", "country");
            ClaimActions.MapJsonKey("urn:meetup:localized_country_name", "localized_country_name");
            ClaimActions.MapJsonKey("urn:meetup:state", "state");

			// Map user's photo information returned as a nested object
			ClaimActions.MapJsonSubKey("urn:meetup:photo.id", "photo", "id");
			ClaimActions.MapJsonSubKey("urn:meetup:photo.highres_link", "photo", "highres_link");
			ClaimActions.MapJsonSubKey("urn:meetup:photo.photo_link", "photo", "photo_link");
			ClaimActions.MapJsonSubKey("urn:meetup:photo.thumb_link", "photo", "thumb_link");
			ClaimActions.MapJsonSubKey("urn:meetup:photo.base_url", "photo", "base_url");
			ClaimActions.MapJsonSubKey("urn:meetup:photo.type", "photo", "type");
        }
    }
}
