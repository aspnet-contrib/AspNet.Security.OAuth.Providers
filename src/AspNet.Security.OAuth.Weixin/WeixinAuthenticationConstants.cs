/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Weixin
{
    /// <summary>
    /// Contains constants specific to the <see cref="WeixinAuthenticationHandler"/>.
    /// </summary>
    public static class WeixinAuthenticationConstants
    {
        public static class Claims
        {
            public const string City = "urn:weixin:city";
            public const string HeadImgUrl = "urn:weixin:headimgurl";
            public const string OpenId = "urn:weixin:openid";
            public const string Privilege = "urn:weixin:privilege";
            public const string Province = "urn:weixin:province";
        }
    }
}
