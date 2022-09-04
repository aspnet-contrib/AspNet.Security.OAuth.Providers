/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Xumm.XummAuthenticationConstants;

namespace AspNet.Security.OAuth.Xumm;

/// <summary>
/// Defines a set of options used by <see cref="XummAuthenticationHandler"/>.
/// </summary>
public class XummAuthenticationOptions : OAuthOptions
{
    public XummAuthenticationOptions()
    {
        ClaimsIssuer = XummAuthenticationDefaults.Issuer;

        CallbackPath = XummAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = XummAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = XummAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = XummAuthenticationDefaults.UserInformationEndpoint;

        UsePkce = true;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(Claims.Picture, "picture");
    }
}
