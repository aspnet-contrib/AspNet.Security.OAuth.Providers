/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Shopify
{
    /// <inheritdoc />
    public class ShopifyAuthenticationOptions : OAuthOptions
    {
        public string ShopifyScope { get; set; } = "read_customers";

        /// <summary>
        /// An alias for ClientId
        /// </summary>
        public string ApiKey
        {
            get => ClientId;
            set => ClientId = value;
        }

        /// <summary>
        /// An alias for ClientSecret
        /// </summary>
        public string ApiSecretKey
        {
            get => ClientSecret;
            set => ClientSecret = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAuthenticationOptions()
        {
            ClaimsIssuer = ShopifyAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(ShopifyAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = ShopifyAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = ShopifyAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = ShopifyAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add(ShopifyScope);

            // ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            // ClaimActions.MapJsonKey(ClaimTypes.Name, "full_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Dns, "shop", "myshopify_domain");
            ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "shop", "id", "long");
        }
    }
}