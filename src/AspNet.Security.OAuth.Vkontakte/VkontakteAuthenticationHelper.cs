using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Vkontakte {
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Facebook after a successful authentication process.
    /// </summary>
    internal static class VkontakteAuthenticationHelper {
        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public static string GetId([NotNull] JObject user) => user.Value<string>("uid");

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public static string GetFirstName([NotNull] JObject user) => user.Value<string>("first_name");

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public static string GetLastName([NotNull] JObject user) => user.Value<string>("last_name");

        /// <summary>
        /// Gets the screen name.
        /// </summary>
        public static string GetScreenName([NotNull] JObject user) => user.Value<string>("screen_name");

        /// <summary>
        /// Gets the photo
        /// </summary>
        public static string GetPhoto([NotNull] JObject user) => user.Value<string>("photo_50");
    }
}