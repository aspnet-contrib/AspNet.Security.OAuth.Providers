﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Shopify
{
    /// <inheritdoc />
    /// <summary>
    /// Substitue for <see cref="AuthenticationProperties"/> to enforce setting shop name
    /// before Challenge and provide an override for <see cref="OAuthOptions.Scope"/>. You
    /// can accomplish the same thing by setting the approriate values in <see cref="AuthenticationProperties.Items"/>.
    /// </summary>
    public class ShopifyAuthenticationProperties : AuthenticationProperties
    {

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="shopName">The name of the shop. Unlike most OAuth providers, the Shop name needs to be known in order
        /// to authorize. This must either be gotten from the user or sent from Shopify during App store
        /// installation.
        /// </param>
        public ShopifyAuthenticationProperties(string shopName)
        {
            ShopName = shopName;
        }

        /// <summary>
        /// Override <see cref="OAuthOptions.Scope"/>. Must be fully formatted.
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
                var prop = GetProperty(ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty);
                return prop != null && prop.Equals(ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue);
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
        private string ShopName
        {
            set => SetProperty(ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty, value);
        }

        private void SetProperty(string propName, string value)
        {
            if (Items.ContainsKey(propName))
                Items[propName] = value;
            else
                Items.Add(propName, value);
        }

        private string GetProperty(string propName)
        {
            return Items.TryGetValue(propName, out var val) ? val : null;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="shopName">The name of the shop. Unlike most OAuth providers, the Shop name needs to be known in order
        /// to authorize. This must either be gotten from the user or sent from Shopify during App store
        /// installation.
        /// </param>
        /// <param name="items">Set Items values.</param>
        public ShopifyAuthenticationProperties(string shopName, IDictionary<string, string> items) : base(items)
        {
            ShopName = shopName;
        }
    }
}
