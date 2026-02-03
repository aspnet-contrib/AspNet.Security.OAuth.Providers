/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using static AspNet.Security.OAuth.Kick.KickAuthenticationConstants;

namespace AspNet.Security.OAuth.Kick;

/// <summary>
/// Defines a set of options used by <see cref="KickAuthenticationHandler"/>.
/// </summary>
public class KickAuthenticationOptions : OAuthOptions
{
    public KickAuthenticationOptions()
    {
        ClaimsIssuer = KickAuthenticationDefaults.Issuer;
        CallbackPath = KickAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = KickAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = KickAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = KickAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user:read");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => GetData(user, "user_id"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => GetData(user, "name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => GetData(user, "email"));
        ClaimActions.MapCustomJson(Claims.ProfilePicture, user => GetData(user, "profile_picture"));

        // Kick requires PKCE (OAuth 2.1)
        UsePkce = true;
    }

    private static string? GetData(JsonElement user, string key)
    {
        if (!user.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        return data.EnumerateArray()
            .Select(p => p.GetString(key))
            .FirstOrDefault();
    }
}
