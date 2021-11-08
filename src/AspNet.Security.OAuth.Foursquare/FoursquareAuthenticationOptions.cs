/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Foursquare;

/// <summary>
/// Defines a set of options used by <see cref="FoursquareAuthenticationHandler"/>.
/// </summary>
public class FoursquareAuthenticationOptions : OAuthOptions
{
    public FoursquareAuthenticationOptions()
    {
        ClaimsIssuer = FoursquareAuthenticationDefaults.Issuer;
        CallbackPath = FoursquareAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = FoursquareAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = FoursquareAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = FoursquareAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
        ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
        ClaimActions.MapJsonKey(ClaimTypes.Uri, "canonicalUrl");
        ClaimActions.MapJsonSubKey(ClaimTypes.Email, "contact", "email");
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => $"{user.GetString("firstName")} {user.GetString("lastName")}".Trim());
    }

    /// <summary>
    /// Gets or sets the API version used when communicating with Foursquare.
    /// See https://developer.foursquare.com/overview/versioning for more information.
    /// </summary>
    public string ApiVersion { get; set; } = FoursquareAuthenticationDefaults.ApiVersion;
}
