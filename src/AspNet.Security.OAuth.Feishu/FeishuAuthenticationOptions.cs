/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Feishu.FeishuAuthenticationConstants;

namespace AspNet.Security.OAuth.Feishu;

/// <summary>
/// Defines a set of options used by <see cref="FeishuAuthenticationHandler"/>.
/// </summary>
public class FeishuAuthenticationOptions : OAuthOptions
{
    public FeishuAuthenticationOptions()
    {
        ClaimsIssuer = FeishuAuthenticationDefaults.Issuer;
        CallbackPath = FeishuAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = FeishuAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = FeishuAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = FeishuAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "open_id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(Claims.AvatarBig, "avatar_big");
        ClaimActions.MapJsonKey(Claims.AvatarMiddle, "avatar_middle");
        ClaimActions.MapJsonKey(Claims.AvatarThumb, "avatar_thumb");
        ClaimActions.MapJsonKey(Claims.AvatarUrl, "avatar_url");
        ClaimActions.MapJsonKey(Claims.Email, "email");
        ClaimActions.MapJsonKey(Claims.EmployeeNo, "employee_no");
        ClaimActions.MapJsonKey(Claims.EnName, "en_name");
        ClaimActions.MapJsonKey(Claims.Mobile, "mobile");
        ClaimActions.MapJsonKey(Claims.Name, "name");
        ClaimActions.MapJsonKey(Claims.OpenId, "open_id");
        ClaimActions.MapJsonKey(Claims.Picture, "picture");
        ClaimActions.MapJsonKey(Claims.Sub, "sub");
        ClaimActions.MapJsonKey(Claims.TenantKey, "tenant_key");
        ClaimActions.MapJsonKey(Claims.UserId, "user_id");
        ClaimActions.MapJsonKey(Claims.UnionId, "union_id");
        ClaimActions.MapJsonKey(Claims.Avatar, "avatar_big");
    }
}
