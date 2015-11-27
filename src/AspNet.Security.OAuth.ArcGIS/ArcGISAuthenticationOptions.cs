/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.ArcGIS {
    /// <summary>
    /// Defines a set of options used by <see cref="ArcGISAuthenticationHandler"/>.
    /// </summary>
    public class ArcGISAuthenticationOptions : OAuthOptions {
        public ArcGISAuthenticationOptions() {
            AuthenticationScheme = ArcGISAuthenticationDefaults.AuthenticationScheme;
            DisplayName = ArcGISAuthenticationDefaults.DisplayName;
            ClaimsIssuer = ArcGISAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(ArcGISAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = ArcGISAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = ArcGISAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = ArcGISAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }
    }
}
