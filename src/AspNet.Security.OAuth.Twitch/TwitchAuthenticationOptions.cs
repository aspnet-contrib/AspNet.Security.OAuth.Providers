/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://Github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Twitch {

    /// <summary>
    /// Defines a set of options used by <see cref="TwitchAuthenticationHandler"/>.
    /// </summary>
    public class TwitchAuthenticationOptions : OAuthAuthenticationOptions {

        public TwitchAuthenticationOptions() {
            AuthenticationScheme = TwitchAuthenticationDefaults.AuthenticationScheme;
            Caption = TwitchAuthenticationDefaults.Caption;
            ClaimsIssuer = TwitchAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(TwitchAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = TwitchAuthenticationDefaults.AuthorizationEndPoint;
            TokenEndpoint = TwitchAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = TwitchAuthenticationDefaults.UserInformationEndpoint;
            SaveTokensAsClaims = false;
            Scope.Add("user_read");
        }
    }
}