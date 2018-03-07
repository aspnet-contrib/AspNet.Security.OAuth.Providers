﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Vkontakte
{
    /// <summary>
    /// Defines a set of options used by <see cref="VkontakteAuthenticationHandler"/>.
    /// </summary>
    public class VkontakteAuthenticationOptions : OAuthOptions
    {
        public VkontakteAuthenticationOptions()
        {
            ClaimsIssuer = VkontakteAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(VkontakteAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = VkontakteAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Hash, "hash");
            ClaimActions.MapJsonKey("urn:vkontakte:photo:link", "photo");
            ClaimActions.MapJsonKey("urn:vkontakte:photo_thumb:link", "photo_rec");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://vk.com/dev/fields for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "uid",
            "first_name",
            "last_name",
            "photo_rec",
            "photo",
            "hash"
        };
    }
}