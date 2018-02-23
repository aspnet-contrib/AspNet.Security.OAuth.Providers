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
            ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey("urn:meetup:name", "name");
            ClaimActions.MapJsonKey("urn:meetup:url", "url");
        }
    }
}
