/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.SoundCloud.SoundCloudAuthenticationConstants;

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
            ClaimActions.MapJsonKey(Claims.FullName, "full_name");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.ProfileUrl, "permalink_url");
        }
    }
}
