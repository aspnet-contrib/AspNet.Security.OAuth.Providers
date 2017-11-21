/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.SoundCloud
{
    /// <summary>
    /// Defines a set of options used by <see cref="SoundCloudAuthenticationHandler"/>.
    /// </summary>
    public class SoundCloudAuthenticationOptions : OAuthOptions
    {
        public SoundCloudAuthenticationOptions()
        {
            ClaimsIssuer = SoundCloudAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SoundCloudAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SoundCloudAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SoundCloudAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = SoundCloudAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey("urn:soundcloud:fullname", "full_name");
            ClaimActions.MapJsonKey("urn:soundcloud:city", "city");
            ClaimActions.MapJsonKey("urn:soundcloud:profileurl", "permalink_url");
        }
    }
}
