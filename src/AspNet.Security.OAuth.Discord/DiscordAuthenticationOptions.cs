/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Discord.DiscordAuthenticationConstants;

namespace AspNet.Security.OAuth.Discord
{
    /// <summary>
    /// Defines a set of options used by <see cref="DiscordAuthenticationHandler"/>.
    /// </summary>
    public class DiscordAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Root Discord CDN URL path
        /// </summary>
        public string DiscordCdn { get; set; } = Urls.DiscordCdn;

        /// <summary>
        /// URL format of the user avatar, using string.format. Substitute {0} for DiscordCdn, {1} for User ID and {2} for Avatar hash. Default of "{0}/avatars/{1}/{2}.png"
        /// </summary>
        public string DiscordAvatarFormat { get; set; } = Urls.AvatarUrlFormat;

        public DiscordAuthenticationOptions()
        {
            ClaimsIssuer = DiscordAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(DiscordAuthenticationDefaults.CallbackPath);
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
