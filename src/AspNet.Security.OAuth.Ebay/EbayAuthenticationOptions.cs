/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Ebay
{
    /// <summary>
    /// Defines a set of options used by <see cref="EbayAuthenticationHandler"/>.
    /// </summary>
    public class EbayAuthenticationOptions : OAuthOptions
    {
        public EbayAuthenticationOptions()
        {
            ClaimsIssuer = EbayAuthenticationDefaults.Issuer;
            CallbackPath = EbayAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = EbayAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = EbayAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = EbayAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("commerce.identity.readonly");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        }

        /// <summary>
        /// Gets or sets the redirect URI of the application, also known as the &quot;RuName&quot;. See the official documentation to retrieve the value: <see href="https://developer.ebay.com/api-docs/static/oauth-redirect-uri.html"/>.
        /// </summary>
        public string RuName { get; set; } = default!;
    }
}
