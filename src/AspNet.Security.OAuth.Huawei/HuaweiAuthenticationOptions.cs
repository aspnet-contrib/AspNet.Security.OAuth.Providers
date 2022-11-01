/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Huawei.HuaweiAuthenticationConstants;

namespace AspNet.Security.OAuth.Huawei;

/// <summary>
/// Defines a set of options used by <see cref="HuaweiAuthenticationHandler"/>.
/// </summary>
public class HuaweiAuthenticationOptions : OAuthOptions
{
    public HuaweiAuthenticationOptions()
    {
        ClaimsIssuer = HuaweiAuthenticationDefaults.Issuer;
        CallbackPath = HuaweiAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = HuaweiAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = HuaweiAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = HuaweiAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openID");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(Claims.Avatar, "headPictureURL");

        Scope.Add("openid");
    }

    public bool FetchNickName { get; set; }
}
