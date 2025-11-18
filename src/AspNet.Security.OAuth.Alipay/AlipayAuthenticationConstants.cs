/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// Contains constants specific to the <see cref="AlipayAuthenticationHandler"/>.
/// </summary>
public static class AlipayAuthenticationConstants
{
    public static class Claims
    {
        public const string Avatar = "urn:alipay:avatar";

        public const string Province = "urn:alipay:province";

        public const string City = "urn:alipay:city";

        public const string Nickname = "urn:alipay:nick_name";

        /// <summary>
        /// The user's gender. F: Female; M: Male.
        /// </summary>
        public const string Gender = "urn:alipay:gender";

        /// <summary>
        /// OpenID is the unique identifier of Alipay users in the application dimension.
        /// See https://opendocs.alipay.com/mini/0ai2i6
        /// </summary>
        public const string OpenId = "urn:alipay:open_id";

        /// <summary>
        /// Alipay user system internal identifier, will no longer be independently open in the future, and will be replaced by OpenID.
        /// </summary>
        public const string UserId = "urn:alipay:user_id";
    }
}
