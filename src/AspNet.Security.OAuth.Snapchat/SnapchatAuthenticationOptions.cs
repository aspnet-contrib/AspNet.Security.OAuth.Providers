/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Snapchat.SnapchatAuthenticationConstants;

namespace AspNet.Security.OAuth.Snapchat;

/// <summary>
/// Defines a set of options used by <see cref="SnapchatAuthenticationHandler"/>.
/// </summary>
public class SnapchatAuthenticationOptions : OAuthOptions
{
    public SnapchatAuthenticationOptions()
    {
        ClaimsIssuer = SnapchatAuthenticationDefaults.Issuer;

        CallbackPath = SnapchatAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = SnapchatAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = SnapchatAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = SnapchatAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "me", "id");
        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "me", "display_name");
        ClaimActions.MapJsonSubKey(ClaimTypes.Email, "me", "email");
        ClaimActions.MapJsonSubKey(Claims.UserId, "me", "id");
        ClaimActions.MapJsonSubKey(Claims.TeamId, "me", "organization_id");
        ClaimActions.MapJsonSubKey(Claims.MemberStatus, "me", "member_status");
        Scope.Add("snapchat-marketing-api");
    }
}
