﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Alipay.AlipayAuthenticationConstants;

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// Defines a set of options used by <see cref="AlipayAuthenticationHandler"/>.
/// </summary>
public class AlipayAuthenticationOptions : OAuthOptions
{
    public AlipayAuthenticationOptions()
    {
        ClaimsIssuer = AlipayAuthenticationDefaults.Issuer;
        CallbackPath = AlipayAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AlipayAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AlipayAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AlipayAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("auth_user");

        ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
        ClaimActions.MapJsonKey(Claims.City, "city");
        ClaimActions.MapJsonKey(Claims.Gender, "gender");
        ClaimActions.MapJsonKey(Claims.Nickname, "nick_name");
        ClaimActions.MapJsonKey(Claims.Province, "province");
    }
}
