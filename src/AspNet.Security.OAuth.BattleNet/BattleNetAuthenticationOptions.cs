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

namespace AspNet.Security.OAuth.BattleNet
{
    /// <summary>
    /// Defines a set of options used by <see cref="BattleNetAuthenticationHandler"/>.
    /// </summary>
    public class BattleNetAuthenticationOptions : OAuthOptions
    {
        public BattleNetAuthenticationOptions()
        {
            ClaimsIssuer = BattleNetAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BattleNetAuthenticationDefaults.CallbackPath);

            Region = BattleNetAuthenticationRegion.America;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "battletag");
        }

        /// <summary>
        /// Sets the region used to determine the appropriate API endpoints when communicating
        /// with BattleNet (by default, <see cref="BattleNetAuthenticationRegion.America"/>).
        /// </summary>
        public BattleNetAuthenticationRegion Region
        {
            set
            {
                switch (value)
                {
                    case BattleNetAuthenticationRegion.America:
                        AuthorizationEndpoint = BattleNetAuthenticationDefaults.America.AuthorizationEndpoint;
                        TokenEndpoint = BattleNetAuthenticationDefaults.America.TokenEndpoint;
                        UserInformationEndpoint = BattleNetAuthenticationDefaults.America.UserInformationEndpoint;
                        break;

                    case BattleNetAuthenticationRegion.China:
                        AuthorizationEndpoint = BattleNetAuthenticationDefaults.China.AuthorizationEndpoint;
                        TokenEndpoint = BattleNetAuthenticationDefaults.China.TokenEndpoint;
                        UserInformationEndpoint = BattleNetAuthenticationDefaults.China.UserInformationEndpoint;
                        break;

                    case BattleNetAuthenticationRegion.Europe:
                        AuthorizationEndpoint = BattleNetAuthenticationDefaults.Europe.AuthorizationEndpoint;
                        TokenEndpoint = BattleNetAuthenticationDefaults.Europe.TokenEndpoint;
                        UserInformationEndpoint = BattleNetAuthenticationDefaults.Europe.UserInformationEndpoint;
                        break;

                    case BattleNetAuthenticationRegion.Korea:
                        AuthorizationEndpoint = BattleNetAuthenticationDefaults.Korea.AuthorizationEndpoint;
                        TokenEndpoint = BattleNetAuthenticationDefaults.Korea.TokenEndpoint;
                        UserInformationEndpoint = BattleNetAuthenticationDefaults.Korea.UserInformationEndpoint;
                        break;

                    case BattleNetAuthenticationRegion.Taiwan:
                        AuthorizationEndpoint = BattleNetAuthenticationDefaults.Taiwan.AuthorizationEndpoint;
                        TokenEndpoint = BattleNetAuthenticationDefaults.Taiwan.TokenEndpoint;
                        UserInformationEndpoint = BattleNetAuthenticationDefaults.Taiwan.UserInformationEndpoint;
                        break;

                    default:
                        throw new ArgumentException($"Region '{value}' is unsupported.", nameof(value));
                }
            }
        }
    }
}
