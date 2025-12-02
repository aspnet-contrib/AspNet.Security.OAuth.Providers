/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Etsy;

/// <summary>
/// Contains constants specific to the <see cref="EtsyAuthenticationHandler"/>.
/// </summary>
public static class EtsyAuthenticationConstants
{
    /// <summary>
    /// Contains claim type constants specific to Etsy authentication.
    /// </summary>
    public static class Claims
    {
        /// <summary>The claim type for the user's Etsy shop ID.</summary>
        public static readonly string ShopId = "urn:etsy:shop_id";

        /// <summary>The claim type for the user's profile image URL.</summary>
        public static readonly string ImageUrl = "urn:etsy:image_url";
    }

    /// <summary>
    /// Contains <see href="https://developers.etsy.com/documentation/reference#section/Authentication/oauth2">Etsy OAuth Scopes</see> constants for Etsy authentication.
    /// </summary>
    public static class Scopes
    {
        /// <summary>See billing and shipping addresses</summary>
        public static readonly string AddressRead = "address_r";

        /// <summary>Update billing and shipping addresses</summary>
        public static readonly string AddressWrite = "address_w";

        /// <summary>See all billing statement data</summary>
        public static readonly string BillingRead = "billing_r";

        /// <summary>Read shopping carts</summary>
        public static readonly string CartRead = "cart_r";

        /// <summary>Add/Remove from shopping carts</summary>
        public static readonly string CartWrite = "cart_w";

        /// <summary>Read a user profile</summary>
        public static readonly string EmailRead = "email_r";

        /// <summary>See private favorites</summary>
        public static readonly string FavoritesRead = "favorites_r";

        /// <summary>Add/Remove favorites</summary>
        public static readonly string FavoritesWrite = "favorites_w";

        /// <summary>See purchase info in feedback</summary>
        public static readonly string FeedbackRead = "feedback_r";

        /// <summary>Delete listings</summary>
        public static readonly string ListingsDelete = "listings_d";

        /// <summary>See all listings (including expired etc)</summary>
        public static readonly string ListingsRead = "listings_r";

        /// <summary>Create/Edit listings</summary>
        public static readonly string ListingsWrite = "listings_w";

        /// <summary>See all profile data</summary>
        public static readonly string ProfileRead = "profile_r";

        /// <summary>Update user profile, avatar, etc</summary>
        public static readonly string ProfileWrite = "profile_w";

        /// <summary>See recommended listings</summary>
        public static readonly string RecommendRead = "recommend_r";

        /// <summary>Accept/Reject recommended listings</summary>
        public static readonly string RecommendWrite = "recommend_w";

        /// <summary>See private shop info</summary>
        public static readonly string ShopsRead = "shops_r";

        /// <summary>Update shop</summary>
        public static readonly string ShopsWrite = "shops_w";

        /// <summary>See all checkout/payment data</summary>
        public static readonly string TransactionsRead = "transactions_r";

        /// <summary>Update receipts</summary>
        public static readonly string TransactionsWrite = "transactions_w";
    }
}
