/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;
using System;

namespace AspNet.Security.OAuth.BattleNet {
    /// <summary>
    /// Defines a set of options used by <see cref="BattleNetAuthenticationHandler"/>.
    /// Defaults to using US server endpoints
    /// </summary>
    public class BattleNetAuthenticationOptions : OAuthOptions
    {
        public BattleNetAuthenticationOptions() {
            AuthenticationScheme = BattleNetAuthenticationDefaults.AuthenticationScheme;
            Caption = BattleNetAuthenticationDefaults.Caption;
            ClaimsIssuer = BattleNetAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BattleNetAuthenticationDefaults.CallbackPath);

            //Default to US server endpoints
            this.ServerRegion = BattleNetServerRegion.US;

            SaveTokensAsClaims = false;
        }
        
        /// <summary>
        /// Sets the oauth endpoints for the specified region
        /// </summary>
        public BattleNetServerRegion ServerRegion
        {
            set
            {
                string authEndpoint;
                string tokenEndpoint;
                string userEndpoint;
                switch (value)
                {
                    case BattleNetServerRegion.US:
                        authEndpoint = BattleNetAuthenticationDefaults.US.AuthorizationEndpoint;
                        tokenEndpoint = BattleNetAuthenticationDefaults.US.TokenEndpoint;
                        userEndpoint = BattleNetAuthenticationDefaults.US.UserInformationEndpoint;
                        break;
                    case BattleNetServerRegion.CN:
                        authEndpoint = BattleNetAuthenticationDefaults.CN.AuthorizationEndpoint;
                        tokenEndpoint = BattleNetAuthenticationDefaults.CN.TokenEndpoint;
                        userEndpoint = BattleNetAuthenticationDefaults.CN.UserInformationEndpoint;
                        break;
                    case BattleNetServerRegion.EU:
                        authEndpoint = BattleNetAuthenticationDefaults.EU.AuthorizationEndpoint;
                        tokenEndpoint = BattleNetAuthenticationDefaults.EU.TokenEndpoint;
                        userEndpoint = BattleNetAuthenticationDefaults.EU.UserInformationEndpoint;
                        break;
                    case BattleNetServerRegion.KR:
                        authEndpoint = BattleNetAuthenticationDefaults.KR.AuthorizationEndpoint;
                        tokenEndpoint = BattleNetAuthenticationDefaults.KR.TokenEndpoint;
                        userEndpoint = BattleNetAuthenticationDefaults.KR.UserInformationEndpoint;
                        break;
                    case BattleNetServerRegion.TW:
                        authEndpoint = BattleNetAuthenticationDefaults.TW.AuthorizationEndpoint;
                        tokenEndpoint = BattleNetAuthenticationDefaults.TW.TokenEndpoint;
                        userEndpoint = BattleNetAuthenticationDefaults.TW.UserInformationEndpoint;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                AuthorizationEndpoint = authEndpoint;
                TokenEndpoint = tokenEndpoint;
                UserInformationEndpoint = userEndpoint;
            }
        }
    }

    public enum BattleNetServerRegion
    {
        US,
        CN,
        EU,
        KR,
        TW
    }
}
