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
        /// <summary>
        /// An alias for <see cref="OAuthOptions.ClientId"/>
        /// </summary>
        public string ApiKey
        {
            get => ClientId;
            set => ClientId = value;
        }

        /// <summary>
        /// An alias for <see cref="OAuthOptions.ClientSecret"/>
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
            
            ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "shop", "myshopify_domain");
            ClaimActions.MapJsonSubKey(ClaimTypes.Name, "shop", "name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Webpage, "shop", "domain");
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "shop", "email");
            ClaimActions.MapJsonSubKey(ClaimTypes.Country, "shop", "country_code");
            ClaimActions.MapJsonSubKey(ClaimTypes.StateOrProvince, "shop", "province_code");
            ClaimActions.MapJsonSubKey(ClaimTypes.PostalCode, "shop", "zip");
            ClaimActions.MapJsonSubKey(ClaimTypes.Locality, "shop", "primary_locale");
            ClaimActions.MapJsonSubKey(ShopifyAuthenticationDefaults.ShopifyPlanNameClaimType, "shop", "plan_name");
            ClaimActions.MapJsonSubKey(ShopifyAuthenticationDefaults.ShopifyEligableForPaymentsClaimType, "shop", "eligible_for_payments", "boolean");
            ClaimActions.MapJsonSubKey(ShopifyAuthenticationDefaults.ShopifyTimezoneClaimType, "shop", "timezone");
        }
    }
}