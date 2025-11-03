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
    public static class Claims
    {
        public const string UserId = "urn:etsy:user_id";
        public const string ShopId = "urn:etsy:shop_id";
        public const string PrimaryEmail = "urn:etsy:primary_email";
        public const string FirstName = "urn:etsy:first_name";
        public const string LastName = "urn:etsy:last_name";
        public const string ImageUrl = "urn:etsy:image_url";
    }

    public static class Scopes
    {
        /// <summary>Read user profile and email address</summary>
        public const string EmailRead = "email_r";

        /// <summary>Read user's listings</summary>
        public const string ListingsRead = "listings_r";

        /// <summary>Create and edit listings</summary>
        public const string ListingsWrite = "listings_w";

        /// <summary>Delete listings</summary>
        public const string ListingsDelete = "listings_d";

        /// <summary>Read shop information</summary>
        public const string ShopsRead = "shops_r";

        /// <summary>Update shop information</summary>
        public const string ShopsWrite = "shops_w";

        /// <summary>Delete shop information</summary>
        public const string ShopsDelete = "shops_d";

        /// <summary>Read transaction data</summary>
        public const string TransactionsRead = "transactions_r";

        /// <summary>Update transaction data</summary>
        public const string TransactionsWrite = "transactions_w";

        /// <summary>Read billing information</summary>
        public const string BillingRead = "billing_r";

        /// <summary>Read private profile information</summary>
        public const string ProfileRead = "profile_r";

        /// <summary>Update profile information</summary>
        public const string ProfileWrite = "profile_w";

        /// <summary>Read user's addresses</summary>
        public const string AddressRead = "address_r";

        /// <summary>Write user's addresses</summary>
        public const string AddressWrite = "address_w";

        /// <summary>Read user's favorites</summary>
        public const string FavoritesRead = "favorites_r";

        /// <summary>Write user's favorites</summary>
        public const string FavoritesWrite = "favorites_w";

        /// <summary>Read user's feedback</summary>
        public const string FeedbackRead = "feedback_r";

        /// <summary>Read user's shops</summary>
        public const string ShopsMyRead = "shops_my_r";

        /// <summary>Read user's cart</summary>
        public const string CartRead = "cart_r";

        /// <summary>Write user's cart</summary>
        public const string CartWrite = "cart_w";

        /// <summary>Read user's recommendations</summary>
        public const string RecommendRead = "recommend_r";

        /// <summary>Write user's recommendations</summary>
        public const string RecommendWrite = "recommend_w";
    }
}
