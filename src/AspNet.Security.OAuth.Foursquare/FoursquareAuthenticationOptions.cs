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
            DisplayName = FoursquareAuthenticationDefaults.Caption;
            ClaimsIssuer = FoursquareAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(FoursquareAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FoursquareAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FoursquareAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FoursquareAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }

        /// <summary>
        /// A parameter, which is a date in YYYYMMDD format, essentially represents the "version" of the API you expect from Foursquare.
        /// Please refer to "https://developer.foursquare.com/overview/versioning".
        /// </summary>
        public string ApiVersion { get; set; } = FoursquareAuthenticationDefaults.ApiVersion;
    }
}
