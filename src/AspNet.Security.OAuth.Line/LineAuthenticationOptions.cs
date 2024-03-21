/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Line.LineAuthenticationConstants;

namespace AspNet.Security.OAuth.Line;

/// <summary>
/// Defines a set of options used by <see cref="LineAuthenticationHandler"/>.
/// </summary>
public class LineAuthenticationOptions : OAuthOptions
{
    public LineAuthenticationOptions()
    {
        ClaimsIssuer = LineAuthenticationDefaults.Issuer;
        CallbackPath = LineAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = LineAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = LineAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = LineAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("profile");
        Scope.Add("openid");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
        ClaimActions.MapJsonKey(Claims.PictureUrl, "pictureUrl", "url");
    }

    /// <summary>
    /// Used to force the consent screen to be displayed even if the user has already granted all requested permissions.
    /// When set to <c>true</c>, Line displays the consent screen for every authorization request.
    /// When left to <c>false</c>, the consent screen is skipped if the user has already granted.
    /// </summary>
    public bool Prompt { get; set; }

    /// <summary>
    /// Gets or sets the address of the endpoint exposing
    /// the email addresses associated with the logged in user.
    /// </summary>
    public string UserEmailsEndpoint { get; set; } = LineAuthenticationDefaults.UserEmailsEndpoint;
}
