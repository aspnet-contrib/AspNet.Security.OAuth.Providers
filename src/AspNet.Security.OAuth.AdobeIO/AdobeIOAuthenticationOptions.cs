/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.AdobeIO
{
    /// <summary>
    /// Defines a set of options used by <see cref="AdobeIOAuthenticationHandler"/>.
    /// </summary>
    public class AdobeIOAuthenticationOptions : OAuthOptions
    {
        public AdobeIOAuthenticationOptions()
        {
            ClaimsIssuer = AdobeIOAuthenticationDefaults.Issuer;
            CallbackPath = AdobeIOAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = AdobeIOAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AdobeIOAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AdobeIOAuthenticationDefaults.UserInformationEndpoint;
            Scope.Clear();
            Scope.Add("openId,AdobeID");
        }
    }
}
