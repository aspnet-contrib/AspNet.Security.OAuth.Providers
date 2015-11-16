/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Paypal {
    /// <summary>
    /// Defines a set of options used by <see cref="PaypalAuthenticationHandler"/>.
    /// </summary>
    public class PaypalAuthenticationOptions : OAuthOptions {
        public PaypalAuthenticationOptions() {
            AuthenticationScheme = PaypalAuthenticationDefaults.AuthenticationScheme;
            DisplayName = PaypalAuthenticationDefaults.DisplayName;
            ClaimsIssuer = PaypalAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(PaypalAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = PaypalAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = PaypalAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = PaypalAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");

            SaveTokensAsClaims = false;
        }
    }
}
