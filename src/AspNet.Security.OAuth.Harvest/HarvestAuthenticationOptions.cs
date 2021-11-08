/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Harvest;

/// <summary>
/// Defines a set of options used by <see cref="HarvestAuthenticationHandler"/>.
/// </summary>
public class HarvestAuthenticationOptions : OAuthOptions
{
    public HarvestAuthenticationOptions()
    {
        ClaimsIssuer = HarvestAuthenticationDefaults.Issuer;
        CallbackPath = HarvestAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = HarvestAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = HarvestAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = HarvestAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "user", "id");
        ClaimActions.MapJsonSubKey(ClaimTypes.GivenName, "user", "first_name");
        ClaimActions.MapJsonSubKey(ClaimTypes.Surname, "user", "last_name");
        ClaimActions.MapJsonSubKey(ClaimTypes.Email, "user", "email");
        ClaimActions.MapCustomJson(
            ClaimTypes.Name,
            payload =>
            {
                if (!payload.TryGetProperty("user", out var user))
                {
                    return null;
                }

                return $"{user.GetString("first_name")} {user.GetString("last_name")}".Trim();
            });
    }
}
