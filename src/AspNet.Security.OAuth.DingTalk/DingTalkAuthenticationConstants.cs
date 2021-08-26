/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.DingTalk
{
    public static class DingTalkAuthenticationConstants
    {
        public static class Claims
        {
            public const string MainOrgAuthHighLevel = "urn:dingtalk:main_org_auth_high_level";
            public const string Nickname = "urn:dingtalk:nick";
            public const string OpenId = "urn:dingtalk:openid";
            public const string UnionId = "urn:dingtalk:unionid";
            public const string Avatar = "urn:dingtalk:avatar";
            public const string Active = "urn:dingtalk:active";
        }
    }
}
