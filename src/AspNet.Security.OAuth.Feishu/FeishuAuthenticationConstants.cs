/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Feishu;

/// <summary>
/// Contains constants specific to the <see cref="FeishuAuthenticationHandler"/>.
/// </summary>
public static class FeishuAuthenticationConstants
{
    public static class Claims
    {
        public const string UnionId = "urn:feishu:unionid";
        public const string Sub = "urn:feishu:sub";
        public const string Name = "urn:feishu:name";
        public const string Picture = "urn:feishu:picture";
        public const string OpenId = "urn:feishu:open_id";
        public const string EnglishName = "urn:feishu:en_name";
        public const string TenantKey = "urn:feishu:tenant_key";
        public const string Avatar = "urn:feishu:avatar";
        public const string AvatarUrl = "urn:feishu:avatar_url";
        public const string AvatarThumb = "urn:feishu:avatar_thumb";
        public const string AvatarMiddle = "urn:feishu:avatar_middle";
        public const string AvatarBig = "urn:feishu:avatar_big";
        public const string Email = "urn:feishu:email";
        public const string UserId = "urn:feishu:userid";
        public const string EmployeeNumber = "urn:feishu:employee_no";
        public const string Mobile = "urn:feishu:mobile";
    }
}
