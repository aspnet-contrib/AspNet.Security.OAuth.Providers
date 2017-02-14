/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Vkontakte
{
    /// <summary>
    /// Configuration options for <see cref="VkontakteAuthenticationMiddleware"/>.
    /// </summary>
    public class VkontakteAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="VkontakteAuthenticationOptions"/>.
        /// </summary>
        public VkontakteAuthenticationOptions()
        {
            AuthenticationScheme = VkontakteAuthenticationDefault.AuthenticationScheme;
            DisplayName = VkontakteAuthenticationDefault.DisplayName;
            ClaimsIssuer = VkontakteAuthenticationDefault.Issuer;

            CallbackPath = new PathString("/signin-vkontakte");

            AuthorizationEndpoint = VkontakteAuthenticationDefault.AuthorizationEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefault.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefault.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://vk.com/dev/fields for more information.
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string> {
            "uid",
            "first_name",
            "last_name",
            "photo_rec",
            "photo",
            "hash"
        };
    }
}