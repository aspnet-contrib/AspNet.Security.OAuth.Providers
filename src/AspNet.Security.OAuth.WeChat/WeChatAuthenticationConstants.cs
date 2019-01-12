/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WeChat
{
    /// <summary>
    /// Contains constants specific to the <see cref="WeChatAuthenticationHandler"/>.
    /// </summary>
    public static class WeChatAuthenticationConstants
    {
        public static class Claims
        {
            public const string City = "urn:wechat:city";
            public const string HeadImgUrl = "urn:wechat:headimgurl";
            public const string OpenId = "urn:wechat:openid";
            public const string UnionId = "urn:wechat:unionid";
            public const string Privilege = "urn:wechat:privilege";
            public const string Province = "urn:wechat:province";
        }
    }
}
