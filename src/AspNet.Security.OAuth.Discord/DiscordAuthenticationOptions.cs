/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

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
			ClaimActions.MapCustomJson(Claims.AvatarUrl, user => $"{Urls.DiscordCDN}/avatars/{user.GetString("id")}/{user.GetString("avatar")}.png");
			ClaimActions.MapJsonKey(Claims.Discriminator, "discriminator");

            Scope.Add("identify");
        }
    }
}
