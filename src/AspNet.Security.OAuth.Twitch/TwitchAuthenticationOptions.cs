/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Twitch.TwitchAuthenticationConstants;

namespace AspNet.Security.OAuth.Twitch
{
    /// <summary>
    /// Defines a set of options used by <see cref="TwitchAuthenticationHandler"/>.
    /// </summary>
    public class TwitchAuthenticationOptions : OAuthOptions
    {
        public TwitchAuthenticationOptions()
        {
            ClaimsIssuer = TwitchAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(TwitchAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = TwitchAuthenticationDefaults.AuthorizationEndPoint;
            TokenEndpoint = TwitchAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = TwitchAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("user:read:email");

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user =>
            {
                return user["data"]?[0]?["id"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(ClaimTypes.Name, user =>
            {
                return user["data"]?[0]?["login"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(Claims.DisplayName, user =>
            {
                return user["data"]?[0]?["display_name"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(ClaimTypes.Email, user =>
            {
                return user["data"]?[0]?["email"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(Claims.Type, user =>
            {
                return user["data"]?[0]?["type"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(Claims.BroadcasterType, user =>
            {
                return user["data"]?[0]?["broadcaster_type"]?.ToObject<string>();
            });

            ClaimActions.MapCustomJson(Claims.Description, user =>
            {
                return user["data"]?[0]?["description"]?.ToObject<string>();
            });
        }
    }
}
