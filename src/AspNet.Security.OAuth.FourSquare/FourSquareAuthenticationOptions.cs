/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.FourSquare {
    /// <summary>
    /// Defines a set of options used by <see cref="FourSquareAuthenticationHandler"/>.
    /// </summary>
    public class FourSquareAuthenticationOptions : OAuthOptions {
        public FourSquareAuthenticationOptions() {
            AuthenticationScheme = FourSquareAuthenticationDefaults.AuthenticationScheme;            
            Caption = FourSquareAuthenticationDefaults.Caption;
            ClaimsIssuer = FourSquareAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(FourSquareAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FourSquareAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FourSquareAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FourSquareAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }

        /// <summary>
        /// A parameter, which is a date in YYYYMMDD format, essentially represents the "version" of the API you expect from Foursquare.
        /// Please refer to "https://developer.foursquare.com/overview/versioning".
        /// </summary>
        public string ApiVersion { get; set; } = FourSquareAuthenticationDefaults.ApiVersion;
    }
}
