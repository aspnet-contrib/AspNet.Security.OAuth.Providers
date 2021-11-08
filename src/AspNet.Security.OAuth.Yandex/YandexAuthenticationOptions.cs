/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Yandex;

/// <summary>
/// Defines a set of options used by <see cref="YandexAuthenticationHandler"/>.
/// </summary>
public class YandexAuthenticationOptions : OAuthOptions
{
    public YandexAuthenticationOptions()
    {
        ClaimsIssuer = YandexAuthenticationDefaults.Issuer;
        CallbackPath = YandexAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = YandexAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = YandexAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = YandexAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");

        ClaimActions.MapCustomJson(
            ClaimTypes.Email,
            user =>
            {
                if (user.TryGetProperty("emails", out var emails))
                {
                    return emails.EnumerateArray().Select((p) => p.GetString()).FirstOrDefault();
                }

                return null;
            });
    }
}
