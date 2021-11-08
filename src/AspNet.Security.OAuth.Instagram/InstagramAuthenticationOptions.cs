/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Instagram.InstagramAuthenticationConstants;

namespace AspNet.Security.OAuth.Instagram;

/// <summary>
/// Defines a set of options used by <see cref="InstagramAuthenticationHandler"/>.
/// </summary>
public class InstagramAuthenticationOptions : OAuthOptions
{
    public InstagramAuthenticationOptions()
    {
        ClaimsIssuer = InstagramAuthenticationDefaults.Issuer;
        CallbackPath = InstagramAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = InstagramAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = InstagramAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = InstagramAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user_profile");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(Claims.AccountType, "account_type");
        ClaimActions.MapJsonKey(Claims.MediaCount, "media_count");
    }

    /// <summary>
    /// Gets the list of list of fields and edges to retrieve from the user information endpoint.
    /// </summary>
    public ISet<string> Fields { get; } = new HashSet<string>()
        {
            "id",
            "username",
        };
}
