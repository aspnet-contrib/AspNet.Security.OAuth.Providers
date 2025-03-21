/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Bilibili.BilibiliAuthenticationConstants;

namespace AspNet.Security.OAuth.Bilibili;

/// <summary>
/// Defines a set of options used by <see cref="BilibiliAuthenticationHandler"/>.
/// </summary>
public class BilibiliAuthenticationOptions : OAuthOptions
{
    public BilibiliAuthenticationOptions()
    {
        ClaimsIssuer = BilibiliAuthenticationDefaults.Issuer;
        CallbackPath = BilibiliAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = BilibiliAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = BilibiliAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = BilibiliAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(Claims.Face, "face");
    }
}
