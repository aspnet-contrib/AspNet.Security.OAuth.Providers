/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Reddit.RedditAuthenticationConstants;

namespace AspNet.Security.OAuth.Reddit;

/// <summary>
/// Defines a set of options used by <see cref="RedditAuthenticationHandler"/>.
/// </summary>
public class RedditAuthenticationOptions : OAuthOptions
{
    public RedditAuthenticationOptions()
    {
        ClaimsIssuer = RedditAuthenticationDefaults.Issuer;

        CallbackPath = RedditAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = RedditAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = RedditAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = RedditAuthenticationDefaults.UserInformationEndpoint;

        // Add duration=permanent to the authorization request to get an access token that doesn't expire after 1 hour.
        // See https://github.com/reddit/reddit/wiki/OAuth2#authorization for more information.
        AdditionalAuthorizationParameters["duration"] = "permanent";

        Scope.Add("identity");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(Claims.Over18, "over_18");
    }

    /// <summary>
    /// Gets or sets the User Agent string to pass when sending requests to Reddit.
    /// Setting this option is strongly recommended to prevent request throttling.
    /// For more information, visit https://github.com/reddit/reddit/wiki/API.
    /// </summary>
    public string? UserAgent { get; set; }
}
