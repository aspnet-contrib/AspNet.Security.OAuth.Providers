/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
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
            CallbackPath = new PathString(EVEOnlineAuthenticationDefaults.CallbackPath);

            Server = EVEOnlineAuthenticationServer.Tranquility;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "CharacterID");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "CharacterName");
            ClaimActions.MapJsonKey(ClaimTypes.Expiration, "ExpiresOn");
            ClaimActions.MapJsonKey(Claims.Scopes, "Scopes");
        }

        /// <summary>
        /// Sets the server used when communicating with EVE Online
        /// (by default, <see cref="EVEOnlineAuthenticationServer.Tranquility"/>).
        /// </summary>
        public EVEOnlineAuthenticationServer Server
        {
            set
            {
                switch (value)
                {
                    case EVEOnlineAuthenticationServer.Tranquility:
                        AuthorizationEndpoint = EVEOnlineAuthenticationDefaults.Tranquility.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineAuthenticationDefaults.Tranquility.TokenEndpoint;
                        UserInformationEndpoint = EVEOnlineAuthenticationDefaults.Tranquility.UserInformationEndpoint;
                        break;

                    case EVEOnlineAuthenticationServer.Singularity:
                        AuthorizationEndpoint = EVEOnlineAuthenticationDefaults.Singularity.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineAuthenticationDefaults.Singularity.TokenEndpoint;
                        UserInformationEndpoint = EVEOnlineAuthenticationDefaults.Singularity.UserInformationEndpoint;
                        break;

                    default:
                        throw new ArgumentException($"Server '{value}' is unsupported.", nameof(value));
                }
            }
        }
    }
}
