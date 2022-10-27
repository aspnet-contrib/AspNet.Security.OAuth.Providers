/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using static AspNet.Security.OAuth.Smartsheet.SmartsheetAuthenticationConstants;

namespace AspNet.Security.OAuth.Smartsheet;

/// <summary>
/// Defines a set of options used by <see cref="SmartsheetAuthenticationHandler"/>.
/// </summary>
public class SmartsheetAuthenticationOptions : OAuthOptions
{
    public SmartsheetAuthenticationOptions()
    {
        ClaimsIssuer = SmartsheetAuthenticationDefaults.Issuer;

        CallbackPath = SmartsheetAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = SmartsheetAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = SmartsheetAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = SmartsheetAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapCustomJson(Claims.ProfileImage, user => GetProfileImageUri(user));
    }

    private static string GetProfileImageUri(JsonElement user)
    {
        var profileImageUri = user.EnumerateObject()
            .Where(u => u.Name == "profileImage")
            .Select(u => u.Value)
            .Select(p => p.GetProperty("imageId"))
            .Select(i => i.GetString()?.Replace("g:", string.Empty, StringComparison.CurrentCulture))
            .FirstOrDefault();

        return profileImageUri ?? string.Empty;
    }
}
