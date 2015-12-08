/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Foursquare {
    /// <summary>
    /// Defines a set of options used by <see cref="FoursquareAuthenticationHandler"/>.
    /// </summary>
    public class FoursquareAuthenticationOptions : OAuthOptions {
        public FoursquareAuthenticationOptions() {
            AuthenticationScheme = FoursquareAuthenticationDefaults.AuthenticationScheme;            
            DisplayName = FoursquareAuthenticationDefaults.DisplayName;
            ClaimsIssuer = FoursquareAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(FoursquareAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FoursquareAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FoursquareAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FoursquareAuthenticationDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the API version used when communicating with Foursquare.
        /// See https://developer.foursquare.com/overview/versioning for more information.
        /// </summary>
        public string ApiVersion { get; set; } = FoursquareAuthenticationDefaults.ApiVersion;
    }
}
