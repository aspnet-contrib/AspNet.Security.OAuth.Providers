/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.WordPress {
    /// <summary>
    /// Defines a set of options used by <see cref="WordPressAuthenticationHandler"/>.
    /// </summary>
    public class WordPressAuthenticationOptions : OAuthOptions {
        public WordPressAuthenticationOptions() {
            AuthenticationScheme = WordPressAuthenticationDefaults.AuthenticationScheme;
            DisplayName = WordPressAuthenticationDefaults.Caption;
            ClaimsIssuer = WordPressAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(WordPressAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WordPressAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WordPressAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WordPressAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;

            // Note: limit by default to 'auth' scope only,
            // otherwise too many permissions are requested.
            Scope.Add("auth");
        }
    }
}