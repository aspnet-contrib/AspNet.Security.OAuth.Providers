using Newtonsoft.Json.Linq;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.Weibo
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Facebook after a successful authentication process.
    /// </summary>
    public class WeiboAuthenticationHelper
    {
        /// <summary>
        /// Gets the user's id.
        /// </summary>
        public static string GetId([NotNull] JObject user) => user.Value<string>("id");

        /// <summary>
        /// Gets the user's screen name(display name).
        /// </summary>
        public static string GetScreenName([NotNull] JObject user) => user.Value<string>("screen_name");

        /// <summary>
        /// Gets the user's name.
        /// </summary>
        public static string GetName([NotNull] JObject user) => user.Value<string>("name");

        /// <summary>
        /// Gets the user's profile image url.
        /// </summary>
        public static string GetProfileImageUrl([NotNull] JObject user) => user.Value<string>("profile_image_url");

        /// <summary>
        /// Gets the user's gender.
        /// </summary>
        public static string GetGender([NotNull] JObject user) => user.Value<string>("gender");

        /// <summary>
        /// Gets the user's avatar(large image).
        /// </summary>
        public static string GetAvatarLarge([NotNull] JObject user) => user.Value<string>("avatar_large");

        /// <summary>
        /// Gets the user's avatar(hd image).
        /// </summary>
        public static string GetAvatarHD([NotNull] JObject user) => user.Value<string>("avatar_hd");

        /// <summary>
        /// Gets the user's conver image phone.
        /// </summary>
        public static string GetCoverImagePhone([NotNull] JObject user) => user.Value<string>("cover_image_phone");

        /// <summary>
        /// Gets the user's location.
        /// </summary>
        public static string GetLocation([NotNull] JObject user) => user.Value<string>("location");
    }
}
