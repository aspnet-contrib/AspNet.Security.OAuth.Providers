using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Vkontakte {
    /// <summary>
    /// Configuration options for <see cref="VkontakteAuthenticationMiddleware"/>.
    /// </summary>
    public class VkontakteAuthenticationOptions : OAuthOptions {
        /// <summary>
        /// Initializes a new <see cref="VkontakteAuthenticationOptions"/>.
        /// </summary>
        public VkontakteAuthenticationOptions() {
            AuthenticationScheme = VkontakteAuthenticationDefault.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            CallbackPath = new PathString("/signin-vkontakte");
            AuthorizationEndpoint = VkontakteAuthenticationDefault.AuthorizeEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefault.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefault.GraphApiEndpoint;
            Fields.Add("uid");
            Fields.Add("first_name");
            Fields.Add("last_name");
            Fields.Add("screen_name");
            Fields.Add("photo_50");
        }

        /// <summary>
        /// The list of fields to retrieve from the UserInformationEndpoint.
        /// https://vk.com/dev/fields
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}