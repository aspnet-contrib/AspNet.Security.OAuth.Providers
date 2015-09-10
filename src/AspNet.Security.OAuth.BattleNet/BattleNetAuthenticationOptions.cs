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
    /// </summary>
    public class BattleNetAuthenticationOptions : OAuthAuthenticationOptions {
        public BattleNetAuthenticationOptions() {
            AuthenticationScheme = BattleNetAuthenticationDefaults.AuthenticationScheme;
            Caption = BattleNetAuthenticationDefaults.Caption;
            ClaimsIssuer = BattleNetAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BattleNetAuthenticationDefaults.CallbackPath);

            this.SetServerRegion(ServerRegion.US); //Default to US

            UserInformationEndpoint = BattleNetAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }

        public void SetServerRegion(ServerRegion serverRegion) {
            string authEndpoint;
            string tokenEndpoint;
            switch (serverRegion)
            {
                case ServerRegion.US:
                    authEndpoint = BattleNetAuthenticationDefaults.AuthorizationEndpointUS;
                    tokenEndpoint = BattleNetAuthenticationDefaults.TokenEndpointUS;
                    break;
                case ServerRegion.TW:
                    authEndpoint = BattleNetAuthenticationDefaults.AuthorizationEndpointTW;
                    tokenEndpoint = BattleNetAuthenticationDefaults.TokenEndpointTW;
                    break;
                case ServerRegion.KR:
                    authEndpoint = BattleNetAuthenticationDefaults.AuthorizationEndpointKR;
                    tokenEndpoint = BattleNetAuthenticationDefaults.TokenEndpointKR;
                    break;
                case ServerRegion.EU:
                    authEndpoint = BattleNetAuthenticationDefaults.AuthorizationEndpointEU;
                    tokenEndpoint = BattleNetAuthenticationDefaults.TokenEndpointEU;
                    break;
                case ServerRegion.CN:
                    authEndpoint = BattleNetAuthenticationDefaults.AuthorizationEndpointCN;
                    tokenEndpoint = BattleNetAuthenticationDefaults.TokenEndpointCN;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverRegion));
            }
            AuthorizationEndpoint = authEndpoint;
            TokenEndpoint = tokenEndpoint;
        }
    }
}
