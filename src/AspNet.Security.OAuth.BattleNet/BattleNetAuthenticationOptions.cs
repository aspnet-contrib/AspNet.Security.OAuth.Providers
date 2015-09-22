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
    public class BattleNetAuthenticationOptions : OAuthAuthenticationOptions
    {
        public BattleNetAuthenticationOptions() {
            AuthenticationScheme = BattleNetAuthenticationDefaults.AuthenticationScheme;
            Caption = BattleNetAuthenticationDefaults.Caption;
            ClaimsIssuer = BattleNetAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BattleNetAuthenticationDefaults.CallbackPath);

            //Default to US server endpoints
            AuthorizationEndpoint = BattleNetAuthenticationDefaults.US.AuthorizationEndpoint;
            TokenEndpoint = BattleNetAuthenticationDefaults.US.TokenEndpoint;

            UserInformationEndpoint = BattleNetAuthenticationDefaults.US.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }
    }
}
