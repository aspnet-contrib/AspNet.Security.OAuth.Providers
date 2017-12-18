/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Shopify;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ShopifyAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="ShopifyAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Shopify authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddShopify([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddShopify(ShopifyAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="ShopifyAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Shopify authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddShopify(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<ShopifyAuthenticationOptions> configuration)
        {
            return builder.AddShopify(ShopifyAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="ShopifyAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Shopify authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Shopify options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddShopify(
            [NotNull] this AuthenticationBuilder builder, 
            [NotNull] string scheme,
            [NotNull] Action<ShopifyAuthenticationOptions> configuration)
        {
            return builder.AddShopify(scheme, ShopifyAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="ShopifyAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Shopify authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Shopify options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddShopify(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, 
            [CanBeNull] string caption,
            [NotNull] Action<ShopifyAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<ShopifyAuthenticationOptions, ShopifyAuthenticationHandler>(scheme, caption, configuration);
        }

    }
}