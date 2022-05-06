/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Xiaomi.XiaomiAuthenticationConstants;

namespace AspNet.Security.OAuth.Xiaomi;

/// <summary>
/// Defines a set of options used by <see cref="XiaomiAuthenticationHandler"/>.
/// </summary>
public class XiaomiAuthenticationOptions : OAuthOptions
{
    public XiaomiAuthenticationOptions()
    {
        ClaimsIssuer = XiaomiAuthenticationDefaults.Issuer;
        CallbackPath = XiaomiAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = XiaomiAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = XiaomiAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = XiaomiAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.Name, "miliaoNick");
        ClaimActions.MapJsonKey(Claims.MiliaoNick, "miliaoNick");
        ClaimActions.MapJsonKey(Claims.UnionId, "unionId");
        ClaimActions.MapJsonKey(Claims.MiliaoIcon, "miliaoIcon");
    }
}
