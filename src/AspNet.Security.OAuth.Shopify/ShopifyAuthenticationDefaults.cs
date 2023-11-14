/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Shopify;

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
    public static readonly string DisplayName = "Shopify";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Shopify";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-shopify";

    /// <summary>
    /// Format value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpointFormat = "https://{0}.myshopify.com/admin/oauth/authorize";

    /// <summary>
    /// Format value for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointFormat = "https://{0}.myshopify.com/admin/oauth/access_token";

    /// <summary>
    /// Format value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointFormat = "https://{0}.myshopify.com/admin/shop";

    /// <summary>
    /// Name of dictionary entry in <see cref="AuthenticationProperties.Items"/> that contains
    /// the name of the shop.
    /// </summary>
    public static readonly string ShopNameAuthenticationProperty = "ShopName";

    /// <summary>
    /// Set this authentication property to override the scope set in <see cref="OAuthOptions.Scope"/>. Note - if this
    /// override is used, it must be fully formatted.
    /// </summary>
    public static readonly string ShopScopeAuthenticationProperty = "Scope";

    /// <summary>
    /// Additional grant options. The only acceptable value is "per-user"
    /// </summary>
    public static readonly string GrantOptionsAuthenticationProperty = "GrantOptions";

    /// <summary>
    /// Per user is the only acceptable grant option at this time.
    /// </summary>
    public static readonly string PerUserAuthenticationPropertyValue = "per-user";

    /// <summary>
    /// The claim type which contains the permission scope returned by Shopify during authorization.
    /// This may not be the same scope requested, so apps should verify they have the scope they need.
    /// </summary>
    public static readonly string ShopifyScopeClaimType = "urn:shopify:scope";

    /// <summary>
    /// The plan name that this shop is using.
    /// </summary>
    public static readonly string ShopifyPlanNameClaimType = "urn:shopify:plan_name";

    /// <summary>
    /// Claim type indicating whether or not this shop if eligible to make payments.
    /// </summary>
    public static readonly string ShopifyEligibleForPaymentsClaimType = "urn:shopify:eligible_for_payments";

    /// <summary>
    /// The time zone that that the shop is using.
    /// </summary>
    public static readonly string ShopifyTimezoneClaimType = "urn:shopify:timezone";
}
