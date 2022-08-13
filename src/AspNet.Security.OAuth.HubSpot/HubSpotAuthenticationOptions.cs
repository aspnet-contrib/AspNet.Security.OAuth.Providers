/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.HubSpot.HubSpotAuthenticationConstants;

namespace AspNet.Security.OAuth.HubSpot;

/// <summary>
/// Defines a set of options used by <see cref="HubSpotAuthenticationHandler"/>.
/// </summary>
public class HubSpotAuthenticationOptions : OAuthOptions
{
    public HubSpotAuthenticationOptions()
    {
        ClaimsIssuer = HubSpotAuthenticationDefaults.Issuer;
        AuthorizationEndpoint = HubSpotAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = HubSpotAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = HubSpotAuthenticationDefaults.UserInformationEndpoint;
        CallbackPath = HubSpotAuthenticationDefaults.CallbackPath;

        ClaimActions.MapJsonKey(ClaimTypes.Name, "user");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "user");
        ClaimActions.MapJsonKey(Claims.HubId, "hub_id");
        ClaimActions.MapJsonKey(Claims.UserId, "user_id");
        ClaimActions.MapJsonKey(Claims.AppId, "app_id");
        ClaimActions.MapJsonKey(Claims.HubDomain, "hub_domain");
    }
}
