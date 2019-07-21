/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Shopify
{
    /// <summary>
    /// Substitue for <see cref="AuthenticationProperties"/> to enforce setting shop name
    /// before Challenge and provide an override for <see cref="OAuthOptions.Scope"/>. You
    /// can accomplish the same thing by setting the approriate values in <see cref="AuthenticationProperties.Items"/>.
    /// </summary>
    public class ShopifyAuthenticationProperties : AuthenticationProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AspNet.Security.OAuth.Shopify.ShopifyAuthenticationProperties" /> class
        /// </summary>
        /// <param name="shopName">The name of the shop. Unlike most OAuth providers, the Shop name needs to be known in order
        /// to authorize. This must either be gotten from the user or sent from Shopify during App store
        /// installation.
        /// </param>
        public ShopifyAuthenticationProperties(string shopName) : this(shopName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AspNet.Security.OAuth.Shopify.ShopifyAuthenticationProperties" /> class
        /// </summary>
        /// <param name="shopName">The name of the shop. Unlike most OAuth providers, the Shop name needs to be known in order
        /// to authorize. This must either be gotten from the user or sent from Shopify during App store
        /// installation.
        /// </param>
        /// <param name="items">Set Items values.</param>
        public ShopifyAuthenticationProperties(string shopName, IDictionary<string, string> items) : base(items)
        {
            SetShopName(shopName);
        }

        /// <summary>
        /// The scope requested. Must be fully formatted. <see cref="OAuthOptions.Scope"/>
        /// </summary>
        public string Scope
        {
            get => GetProperty(ShopifyAuthenticationDefaults.ShopScopeAuthenticationProperty);
            set => SetProperty(ShopifyAuthenticationDefaults.ShopScopeAuthenticationProperty, value);
        }

        /// <summary>
        /// Request a per user token. No offline access, do not persist.
        /// </summary>
        public bool RequestPerUserToken
        {
            get
            {
                string prop = GetProperty(ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty);
                return string.Equals(prop, ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue, StringComparison.OrdinalIgnoreCase);
            }

            set => SetProperty(ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty,
                value ?
                    ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue :
                    null);
        }

        /// <summary>
        /// The name of the shop. Unlike most OAuth providers, the Shop name needs to be known in order
        /// to authorize. This must either be gotten from the user or sent from Shopify during App store
        /// installation.
        /// </summary>
        private void SetShopName(string shopName)
            => SetProperty(ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty, shopName);
    
        private void SetProperty(string propName, string value)
            => Items[propName] = value;

        private string GetProperty(string propName)
        {
            return Items.TryGetValue(propName, out var val) ? val : null;
        }
    }
}
