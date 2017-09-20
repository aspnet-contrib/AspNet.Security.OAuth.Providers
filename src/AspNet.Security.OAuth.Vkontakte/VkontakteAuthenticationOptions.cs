/*
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
    /// Configuration options for <see cref="VkontakteAuthenticationHandler"/>.
    /// </summary>
    public class VkontakteAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="VkontakteAuthenticationOptions"/>.
        /// </summary>
        public VkontakteAuthenticationOptions()
        {
            ClaimsIssuer = VkontakteAuthenticationDefaults.Issuer;

            CallbackPath = new PathString("/signin-vkontakte");

            AuthorizationEndpoint = VkontakteAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "screen_name");
            ClaimActions.MapJsonKey("urn:vkontakte:photo_thumb", "photo_50");
            ClaimActions.MapJsonKey("urn:vkontakte:photo", "photo_max");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://vk.com/dev/objects/user for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "uid",
            "first_name",
            "last_name",
            "screen_name",
            "photo_50",
            "photo_max"
        };
    }
}
