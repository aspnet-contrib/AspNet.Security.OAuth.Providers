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
        ClaimActions.MapJsonKey(Claims.UnionId, "union_id");
        ClaimActions.MapJsonKey(Claims.Avatar, "avatar_big");
    }
}
