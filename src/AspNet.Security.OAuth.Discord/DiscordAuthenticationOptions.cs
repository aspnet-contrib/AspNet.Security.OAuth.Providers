/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Discord.DiscordAuthenticationConstants;

namespace AspNet.Security.OAuth.Discord;

/// <summary>
/// Defines a set of options used by <see cref="DiscordAuthenticationHandler"/>.
/// </summary>
public class DiscordAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Gets or sets a value which controls how the authorization flow handles existing authorizations.
    /// The default value of this property is <see langword="null"/> and the <c>prompt</c> query string
    /// parameter will not be appended to the <see cref="OAuthOptions.AuthorizationEndpoint"/> value.
    /// Assigning this property any value other than <see langword="null"/> or an empty string will
    /// automatically append the <c>prompt</c> query string parameter to the <see cref="OAuthOptions.AuthorizationEndpoint"/>
    /// value, with the specified value.
    /// </summary>
    public string? Prompt { get; set; }

    public DiscordAuthenticationOptions()
    {
        ClaimsIssuer = DiscordAuthenticationDefaults.Issuer;
        CallbackPath = DiscordAuthenticationDefaults.CallbackPath;
        AuthorizationEndpoint = DiscordAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = DiscordAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = DiscordAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(Claims.AvatarHash, "avatar");
        ClaimActions.MapJsonKey(Claims.Discriminator, "discriminator");

        Scope.Add("identify");
    }
}
