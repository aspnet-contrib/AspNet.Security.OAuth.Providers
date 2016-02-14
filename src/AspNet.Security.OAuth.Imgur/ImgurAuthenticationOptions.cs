/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Imgur {
    /// <summary>
    /// Defines a set of options used by <see cref="ImgurAuthenticationHandler"/>.
    /// </summary>
    public class ImgurAuthenticationOptions : OAuthOptions {
        public ImgurAuthenticationOptions() {
            AuthenticationScheme = ImgurAuthenticationDefaults.AuthenticationScheme;
            DisplayName = ImgurAuthenticationDefaults.DisplayName;
            ClaimsIssuer = ImgurAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(ImgurAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = ImgurAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = ImgurAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = ImgurAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
