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
            DisplayName = AuthenticationScheme;
            CallbackPath = new PathString("/signin-vkontakte");
            AuthorizationEndpoint = VkontakteAuthenticationDefault.AuthorizeEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefault.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefault.GraphApiEndpoint;
            Fields = new[] { "uid", "first_name", "last_name", "photo_50", "screen_name" };
        }

        /// <summary>
        /// The list of fields to retrieve from the UserInformationEndpoint.
        /// https://vk.com/dev/fields
        /// </summary>
        public ICollection<string> Fields { get; }
    }
}