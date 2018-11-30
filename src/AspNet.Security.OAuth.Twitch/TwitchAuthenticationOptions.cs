﻿/*
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
            ForceVerify = TwitchAuthenticationDefaults.ForceVerify;

            Scope.Add("user:read:email");

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user =>
            {
                return user["data"]?[0]?.Value<string>("id");
            });

            ClaimActions.MapCustomJson(ClaimTypes.Name, user =>
            {
                return user["data"]?[0]?.Value<string>("login");
            });

            ClaimActions.MapCustomJson(Claims.DisplayName, user =>
            {
                return user["data"]?[0]?.Value<string>("display_name");
            });

            ClaimActions.MapCustomJson(ClaimTypes.Email, user =>
            {
                return user["data"]?[0]?.Value<string>("email");
            });

            ClaimActions.MapCustomJson(Claims.Type, user =>
            {
                return user["data"]?[0]?.Value<string>("type");
            });

            ClaimActions.MapCustomJson(Claims.BroadcasterType, user =>
            {
                return user["data"]?[0]?.Value<string>("broadcaster_type");
            });

            ClaimActions.MapCustomJson(Claims.Description, user =>
            {
                return user["data"]?[0]?.Value<string>("description");
            });

            ClaimActions.MapCustomJson(Claims.ProfileImageUrl, user =>
            {
                return user["data"]?[0]?.Value<string>("profile_image_url");
            });

            ClaimActions.MapCustomJson(Claims.OfflineImageUrl, user =>
            {
                return user["data"]?[0]?.Value<string>("offline_image_url");
            });
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the "force_verify=true" flag should be sent to Twitch.
        /// When set to <c>true</c>, Twitch displays the consent screen for every authorization request.
        /// When left to <c>false</c>, the consent screen is skipped if the user is already logged in.
        /// </summary>
        public bool ForceVerify { get; set; }
    }
}
