/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Discord.DiscordAuthenticationConstants;

namespace AspNet.Security.OAuth.Discord
{
    /// <summary>
    /// Defines a set of options used by <see cref="DiscordAuthenticationHandler"/>.
    /// </summary>
    public class DiscordAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Gets or sets the root Discord CDN URL path. The default value is <see cref="Urls.DiscordCdn"/>.
        /// </summary>
        public string DiscordCdn { get; set; } = Urls.DiscordCdn;

        /// <summary>
        /// Gets or sets the URL format string for the user avatar URL, using <see cref="string.Format(System.IFormatProvider?, string, object?[])"/>.
        /// Substitute <c>{0}</c> for <see cref="DiscordCdn"/>, <c>{1}</c> for the user ID and <c>{2}</c> for the Avatar hash.
        /// The default value is <see cref="Urls.AvatarUrlFormat"/>.
        /// </summary>
        public string DiscordAvatarFormat { get; set; } = Urls.AvatarUrlFormat;

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
            ClaimActions.MapCustomJson(Claims.AvatarUrl, user =>
                string.Format(
                    CultureInfo.InvariantCulture,
                    DiscordAvatarFormat,
                    DiscordCdn.TrimEnd('/'),
                    user.GetString("id"),
                    user.GetString("avatar")));

            Scope.Add("identify");
        }
    }
}
