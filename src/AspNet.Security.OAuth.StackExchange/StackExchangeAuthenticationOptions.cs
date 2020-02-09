/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.StackExchange.StackExchangeAuthenticationConstants;

namespace AspNet.Security.OAuth.StackExchange
{
    /// <summary>
    /// Defines a set of options used by <see cref="StackExchangeAuthenticationHandler"/>.
    /// </summary>
    public class StackExchangeAuthenticationOptions : OAuthOptions
    {
        public StackExchangeAuthenticationOptions()
        {
            ClaimsIssuer = StackExchangeAuthenticationDefaults.Issuer;

            CallbackPath = StackExchangeAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = StackExchangeAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = StackExchangeAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = StackExchangeAuthenticationDefaults.UserInformationEndpoint;
            BackchannelHttpHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
            ClaimActions.MapJsonKey(ClaimTypes.Webpage, "website_url");
            ClaimActions.MapJsonKey(Claims.Link, "link");
        }

        /// <summary>
        /// Gets or sets the application request key, obtained
        /// when registering your application with StackApps.
        /// </summary>
        public string RequestKey { get; set; }

        /// <summary>
        /// Gets or sets the site on which the user is registered.
        /// By default, this property is set to "StackOverflow".
        /// </summary>
        public string Site { get; set; } = "StackOverflow";
    }
}
