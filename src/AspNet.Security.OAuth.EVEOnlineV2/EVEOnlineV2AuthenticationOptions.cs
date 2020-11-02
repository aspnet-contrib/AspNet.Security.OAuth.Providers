/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.EVEOnlineV2
{
    /// <summary>
    /// Defines a set of options used by <see cref="EVEOnlineV2AuthenticationHandler"/>.
    /// </summary>
    public class EVEOnlineV2AuthenticationOptions : OAuthOptions
    {
        public EVEOnlineV2AuthenticationOptions()
        {
            ClaimsIssuer = EVEOnlineV2AuthenticationDefaults.Issuer;
            CallbackPath = EVEOnlineV2AuthenticationDefaults.CallbackPath;

            Server = EVEOnlineV2AuthenticationServer.Tranquility;
        }

        /// <summary>
        /// Sets the server used when communicating with EVE Online
        /// (by default, <see cref="EVEOnlineV2AuthenticationServer.Tranquility"/>).
        /// </summary>
        public EVEOnlineV2AuthenticationServer Server
        {
            set
            {
                switch (value)
                {
                    case EVEOnlineV2AuthenticationServer.Tranquility:
                        AuthorizationEndpoint = EVEOnlineV2AuthenticationDefaults.Tranquility.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineV2AuthenticationDefaults.Tranquility.TokenEndpoint;
                        break;

                    case EVEOnlineV2AuthenticationServer.Singularity:
                        AuthorizationEndpoint = EVEOnlineV2AuthenticationDefaults.Singularity.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineV2AuthenticationDefaults.Singularity.TokenEndpoint;
                        break;

                    default:
                        throw new ArgumentException($"Server '{value}' is unsupported.", nameof(value));
                }
            }
        }
    }
}
