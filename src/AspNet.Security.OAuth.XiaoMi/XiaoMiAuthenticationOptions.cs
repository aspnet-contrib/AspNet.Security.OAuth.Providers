/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.XiaoMi.XiaoMiAuthenticationConstants;

namespace AspNet.Security.OAuth.XiaoMi;

/// <summary>
/// Defines a set of options used by <see cref="XiaoMiAuthenticationHandler"/>.
/// </summary>
public class XiaoMiAuthenticationOptions : OAuthOptions
{
    public XiaoMiAuthenticationOptions()
    {
        ClaimsIssuer = XiaoMiAuthenticationDefaults.Issuer;
        CallbackPath = XiaoMiAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = XiaoMiAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = XiaoMiAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = XiaoMiAuthenticationDefaults.UserInformationEndpoint;

        // Scope.Add("1");
        // Scope.Add("3");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "miliaoNick");
        ClaimActions.MapJsonKey(Claims.MiliaoNick, "miliaoNick");
        ClaimActions.MapJsonKey(Claims.UnionId, "unionId");
        ClaimActions.MapJsonKey(Claims.MiliaoIcon, "miliaoIcon");
    }
}
