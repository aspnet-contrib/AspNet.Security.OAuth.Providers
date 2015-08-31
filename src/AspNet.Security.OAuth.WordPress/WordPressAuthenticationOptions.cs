﻿/*
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
    public class WordPressAuthenticationOptions : OAuthAuthenticationOptions {
        public WordPressAuthenticationOptions() {
            AuthenticationScheme = WordPressAuthenticationDefaults.AuthenticationScheme;
            Caption = WordPressAuthenticationDefaults.Caption;
            ClaimsIssuer = WordPressAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(WordPressAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WordPressAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WordPressAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WordPressAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;

            // Limit by default to 'auth' scope only, otherwise too many permissions get requested
            Scope.Add("auth");
        }
    }
}