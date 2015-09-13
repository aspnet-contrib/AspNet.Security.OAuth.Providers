/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Slack {
    /// <summary>
    /// Defines a set of options used by <see cref="SlackAuthenticationHandler"/>.
    /// </summary>
    public class SlackAuthenticationOptions : OAuthOptions {
        public SlackAuthenticationOptions() {
            AuthenticationScheme = SlackAuthenticationDefaults.AuthenticationScheme;
            Caption = SlackAuthenticationDefaults.Caption;
            ClaimsIssuer = SlackAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SlackAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SlackAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SlackAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = SlackAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }
    }
}
