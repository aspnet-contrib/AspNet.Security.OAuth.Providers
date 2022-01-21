/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Trovo.TrovoAuthenticationConstants;

namespace AspNet.Security.OAuth.Trovo;

/// <summary>
/// Defines a set of options used by <see cref="TrovoAuthenticationHandler"/>.
/// </summary>
public class TrovoAuthenticationOptions : OAuthOptions
{
    public TrovoAuthenticationOptions()
    {
        ClaimsIssuer = TrovoAuthenticationDefaults.Issuer;

        CallbackPath = TrovoAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = TrovoAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = TrovoAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = TrovoAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user_details_self");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "nickName");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "userName");
        ClaimActions.MapJsonKey(Claims.ChannelId, "channelId");
        ClaimActions.MapJsonKey(Claims.ProfilePic, "profilePic");
    }
}
