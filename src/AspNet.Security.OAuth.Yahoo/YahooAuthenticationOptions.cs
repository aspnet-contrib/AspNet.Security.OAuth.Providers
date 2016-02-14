/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Yahoo {
    /// <summary>
    /// Defines a set of options used by <see cref="YahooAuthenticationHandler"/>.
    /// </summary>
    public class YahooAuthenticationOptions : OAuthOptions {
        public YahooAuthenticationOptions() {
            AuthenticationScheme = YahooAuthenticationDefaults.AuthenticationScheme;
            DisplayName = YahooAuthenticationDefaults.DisplayName;
            ClaimsIssuer = YahooAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(YahooAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = YahooAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YahooAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YahooAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}