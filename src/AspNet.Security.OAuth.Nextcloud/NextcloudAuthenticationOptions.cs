/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Nextcloud.NextcloudAuthenticationConstants;

namespace AspNet.Security.OAuth.Nextcloud;

public class NextcloudAuthenticationOptions : OAuthOptions
{
    public NextcloudAuthenticationOptions()
    {
        ClaimsIssuer = NextcloudAuthenticationDefaults.Issuer;
        CallbackPath = NextcloudAuthenticationDefaults.CallbackPath;

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => GetDataString(user, "id"));
        ClaimActions.MapCustomJson(Claims.Username, user => GetDataString(user, "id"));
        ClaimActions.MapCustomJson(Claims.DisplayName, user => GetDataString(user, "displayname"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => GetDataString(user, "email"));
        ClaimActions.MapCustomJson(Claims.IsEnabled, user => GetDataString(user, "enabled"));
        ClaimActions.MapCustomJson(Claims.Language, user => GetDataString(user, "language"));
        ClaimActions.MapCustomJson(Claims.Locale, user => GetDataString(user, "locale"));
        ClaimActions.MapCustomJson(
            Claims.Groups,
            user =>
            {
                if (TryGetData(user, out var data) &&
                    data.TryGetProperty("groups", out var groups))
                {
                    return string.Join(',', groups.EnumerateArray().Select((p) => p.GetString()));
                }

                return null;
            });
    }

    private static bool TryGetData(JsonElement user, out JsonElement data)
    {
        if (user.TryGetProperty("ocs", out var ocs) &&
            ocs.TryGetProperty("data", out data))
        {
            return true;
        }

        data = default;
        return false;
    }

    private static string? GetDataString(JsonElement user, string key)
    {
        if (TryGetData(user, out var data))
        {
            return data.GetString(key);
        }

        return null;
    }
}
