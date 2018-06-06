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
using Microsoft.AspNetCore.Http;
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

            CallbackPath = new PathString(StackExchangeAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = StackExchangeAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = StackExchangeAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = StackExchangeAuthenticationDefaults.UserInformationEndpoint;
            BackchannelHttpHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user["items"]?[0]?.Value<string>("account_id"));
            ClaimActions.MapCustomJson(ClaimTypes.Name, user => user["items"]?[0]?.Value<string>("display_name"));
            ClaimActions.MapCustomJson(ClaimTypes.Webpage, user => user["items"]?[0]?.Value<string>("website_url"));
            ClaimActions.MapCustomJson(Claims.Link, user => user["items"]?[0]?.Value<string>("link"));
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
