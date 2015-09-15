/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Fitbit {
    /// <summary>
    /// Defines a set of options used by <see cref="FitbitAuthenticationHandler"/>.
    /// </summary>
    public class FitbitAuthenticationOptions : OAuthAuthenticationOptions {
        public FitbitAuthenticationOptions() {
            AuthenticationScheme = FitbitAuthenticationDefaults.AuthenticationScheme;
            Caption = FitbitAuthenticationDefaults.Caption;
            ClaimsIssuer = FitbitAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(FitbitAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FitbitAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FitbitAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FitbitAuthenticationDefaults.UserInformationEndpoint;

            foreach (var scopeOption in FitbitAuthenticationDefaults.Scope) {
                Scope.Add(scopeOption);
            }
            
            SaveTokensAsClaims = false;
        }
    }
}
