/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.EVEOnline.EVEOnlineAuthenticationConstants;

namespace AspNet.Security.OAuth.EVEOnline
{
    /// <summary>
    /// Defines a set of options used by <see cref="EVEOnlineAuthenticationHandler"/>.
    /// </summary>
    public class EVEOnlineAuthenticationOptions : OAuthOptions
    {
        public EVEOnlineAuthenticationOptions()
        {
            ClaimsIssuer = EVEOnlineAuthenticationDefaults.Issuer;
            CallbackPath = EVEOnlineAuthenticationDefaults.CallbackPath;

            Server = EVEOnlineAuthenticationServer.Tranquility;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "CharacterID");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "CharacterName");
            ClaimActions.MapJsonKey(ClaimTypes.Expiration, "ExpiresOn");
            ClaimActions.MapJsonKey(Claims.Scopes, "Scopes");
        }

        public EVEOnlineOauthVersion Version { get; set; }

        /// <summary>
        /// Sets the server used when communicating with EVE Online
        /// (by default, <see cref="EVEOnlineAuthenticationServer.Tranquility"/>).
        /// </summary>
        public EVEOnlineAuthenticationServer Server
        {
            set
            {
                var baseUrl = string.Empty;
                switch (value)
                {
                    case EVEOnlineAuthenticationServer.Tranquility:
                        baseUrl = EVEOnlineAuthenticationDefaults.TranquilityUrl;
                        break;

                    case EVEOnlineAuthenticationServer.Serenity:
                        baseUrl = EVEOnlineAuthenticationDefaults.SerenityUrl;
                        break;

                    default:
                        throw new ArgumentException($"Server '{value}' is unsupported.", nameof(value));
                }

                var version = string.Empty;
                if (Version == EVEOnlineOauthVersion.V2)
                {
                    version = "/v2";
                }

                AuthorizationEndpoint = $"{baseUrl}{version}/oauth/authorize";
                TokenEndpoint = $"{baseUrl}{version}/oauth/token";
                UserInformationEndpoint = $"{baseUrl}/oauth/verify";
            }
        }
    }
}
