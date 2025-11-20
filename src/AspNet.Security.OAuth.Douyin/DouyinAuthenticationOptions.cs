/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Douyin.DouyinAuthenticationConstants;

namespace AspNet.Security.OAuth.Douyin;

/// <summary>
/// Defines a set of options used by <see cref="DouyinAuthenticationHandler"/>.
/// </summary>
public class DouyinAuthenticationOptions : OAuthOptions
{
    public DouyinAuthenticationOptions()
    {
        ClaimsIssuer = DouyinAuthenticationDefaults.Issuer;
        CallbackPath = DouyinAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = DouyinAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = DouyinAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = DouyinAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user_info");

        ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
        ClaimActions.MapJsonKey(Claims.Nickname, "nickname");
    }
}
