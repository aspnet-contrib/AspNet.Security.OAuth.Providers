/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Shopify
{
    /// <summary>
    /// Default values used by the Shopify authentication middleware.
    /// </summary>
    public static class ShopifyAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Shopify";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Shopify";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Shopify";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-shopify";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string FormatAuthorizationEndpoint = "https://{0}.myshopify.com/admin/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string FormatTokenEndpoint = "https://{0}.myshopify.com/admin/oauth/access_token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string FormatUserInformationEndpoint = "https://{0}.myshopify.com/admin/shop";


        /// <summary>
        /// Name of dictionary entry in <see cref="AuthenticationProperties.Items"/> that contains
        /// the name of the shop.
        /// </summary>
        public const string ShopNameAuthenticationProperty = "ShopName";

        /// <summary>
        /// Set this authentication property to override the scope set in <see cref="OAuthOptions.Scope"/>. Note - if this
        /// override is used, it must be fully formatted.
        /// </summary>
        public const string ShopScopeAuthenticationProperty = "Scope";

        /// <summary>
        /// Additional grant options. The only acceptable value is "per-user"
        /// </summary>
        public const string GrantOptionsAuthenticationProperty = "GrantOptions";

        /// <summary>
        /// Per user is the only acceptable grant option at this time.
        /// </summary>
        public const string PerUserAuthenticationPropertyValue = "per-user";


        /// <summary>
        /// The claim type which contains the permission scope returned by Shopify during authorization.
        /// This may not be the same scope requested, so apps should verify they have the scope they need.
        /// </summary>
        public const string ShopifyScopeClaimType = "urn:shopify:scope";

        /// <summary>
        /// The plan name that this shop is using.
        /// </summary>
        public const string ShopifyPlanNameClaimType = "urn:shopify:plan_name";

        /// <summary>
        /// Claim type indicating whether or not this shop if eligible to make payments.
        /// </summary>
        public const string ShopifyEligibleForPaymentsClaimType = "urn:shopify:eligible_for_payments";

        /// <summary>
        /// The timezone that that the shop is using.
        /// </summary>
        public const string ShopifyTimezoneClaimType = "urn:shopify:timezone";
    }
}
