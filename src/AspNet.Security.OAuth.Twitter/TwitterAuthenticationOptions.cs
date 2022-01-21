/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Twitter.TwitterAuthenticationConstants;

namespace AspNet.Security.OAuth.Twitter;

/// <summary>
/// Defines a set of options used by <see cref="TwitterAuthenticationHandler"/>.
/// </summary>
public class TwitterAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterAuthenticationOptions"/> class.
    /// </summary>
    public TwitterAuthenticationOptions()
    {
        ClaimsIssuer = TwitterAuthenticationDefaults.Issuer;
        CallbackPath = TwitterAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = TwitterAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = TwitterAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = TwitterAuthenticationDefaults.UserInformationEndpoint;

        UsePkce = true;

        Scope.Add("tweet.read");
        Scope.Add("users.read");

        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "data", "username");
        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "data", "id");
        ClaimActions.MapJsonSubKey(Claims.Name, "data", "name");
    }

    /// <summary>
    /// Gets the optional list of additional data objects to expand from the user information endpoint.
    /// </summary>
    public ISet<string> Expansions { get; } = new HashSet<string>();

    /// <summary>
    /// Gets the optional list of tweet fields to retrieve from the user information endpoint.
    /// </summary>
    public ISet<string> TweetFields { get; } = new HashSet<string>();

    /// <summary>
    /// Gets the optional list of user fields to retrieve from the user information endpoint.
    /// </summary>
    public ISet<string> UserFields { get; } = new HashSet<string>();
}
