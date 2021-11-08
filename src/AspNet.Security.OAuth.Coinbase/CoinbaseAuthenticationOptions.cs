/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Coinbase.CoinbaseAuthenticationConstants;

namespace AspNet.Security.OAuth.Coinbase;

/// <summary>
/// Defines a set of options used by <see cref="CoinbaseAuthenticationHandler"/>.
/// </summary>
public class CoinbaseAuthenticationOptions : OAuthOptions
{
    public CoinbaseAuthenticationOptions()
    {
        ClaimsIssuer = CoinbaseAuthenticationDefaults.Issuer;

        CallbackPath = CoinbaseAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = CoinbaseAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = CoinbaseAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = CoinbaseAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

        ClaimActions.MapJsonKey(Claims.Username, "username");
        ClaimActions.MapJsonKey(Claims.ProfileLocation, "profile_location");
        ClaimActions.MapJsonKey(Claims.ProfileBio, "profile_bio");
        ClaimActions.MapJsonKey(Claims.ProfileUrl, "profile_url");
        ClaimActions.MapJsonKey(Claims.AvatarUrl, "avatar_url");
    }
}
