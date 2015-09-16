/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Onshape
{
    /// <summary>
    /// Defines a set of options used by <see cref="OnshapeAuthenticationHandler"/>.
    /// </summary>
    public class OnshapeAuthenticationOptions : OAuthOptions {
        public OnshapeAuthenticationOptions() {
            AuthenticationScheme = OnshapeAuthenticationDefaults.AuthenticationScheme;
            Caption = OnshapeAuthenticationDefaults.Caption;
            ClaimsIssuer = OnshapeAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(OnshapeAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = OnshapeAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OnshapeAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OnshapeAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }
    }
}
