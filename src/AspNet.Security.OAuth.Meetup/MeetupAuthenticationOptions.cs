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
            ClaimActions.MapJsonKey(MeetupClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(MeetupClaimTypes.Status, "status");
            ClaimActions.MapJsonKey(MeetupClaimTypes.Latitude, "lat");
            ClaimActions.MapJsonKey(MeetupClaimTypes.Longitude, "lon");
            ClaimActions.MapJsonKey(MeetupClaimTypes.Joined, "joined");
            ClaimActions.MapJsonKey(MeetupClaimTypes.City, "city");
            ClaimActions.MapJsonKey(MeetupClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(MeetupClaimTypes.LocalizedCountryName, "localized_country_name");
            ClaimActions.MapJsonKey(MeetupClaimTypes.State, "state");

            // Map user's photo information returned as a nested object
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoId, "photo", "id");
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoHighResolutionLink, "photo", "highres_link");
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoLink, "photo", "photo_link");
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoThumbnailLink, "photo", "thumb_link");
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoBaseUrl, "photo", "base_url");
            ClaimActions.MapJsonSubKey(MeetupClaimTypes.PhotoType, "photo", "type");
        }
    }
}
