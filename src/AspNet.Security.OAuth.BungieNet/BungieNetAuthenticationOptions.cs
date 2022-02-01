/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.BungieNet.BungieNetAuthenticationConstants;

namespace AspNet.Security.OAuth.BungieNet;

/// <summary>
/// Defines a set of options used by <see cref="BungieNetAuthenticationHandler"/>.
/// </summary>
public class BungieNetAuthenticationOptions : OAuthOptions
{
    public BungieNetAuthenticationOptions()
    {
        ClaimsIssuer = BungieNetAuthenticationDefaults.Issuer;
        CallbackPath = BungieNetAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = BungieNetAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = BungieNetAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = BungieNetAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "Response", "membershipId");
        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "Response", "displayName");
        ClaimActions.MapJsonSubKey(Claims.ProfilePicturePath, "Response", "profilePicturePath");
        ClaimActions.MapJsonSubKey(Claims.UniqueName, "Response", "uniqueName");
    }

    /// <summary>
    /// API Key associated with the client ID.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <inheritdoc />
    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new ArgumentException($"The '{nameof(ApiKey)}' option must be provided.", nameof(ApiKey));
        }
    }
}
