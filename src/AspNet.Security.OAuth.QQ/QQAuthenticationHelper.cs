/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.QQ
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Facebook after a successful authentication process.
    /// </summary>
    public static class QQAuthenticationHelper
    {
        /// <summary>
        /// Gets the user's nickname.
        /// </summary>
        public static string GetNickname(JObject user) => user.Value<string>("nickname");

        /// <summary>
        /// Gets the user's qq profile picture url(30px*30px).
        /// </summary>
        public static string GetPicture(JObject user) => user.Value<string>("figureurl");

        /// <summary>
        /// Gets the user's qq profile pictureurl(50px*50px).
        /// </summary>
        public static string GetPictureMedium(JObject user) => user.Value<string>("figureurl_1");

        /// <summary>
        /// Gets the user's qq profile picture url(100px*100px).
        /// </summary>
        public static string GetPictureFull(JObject user) => user.Value<string>("figureurl_2");

        /// <summary>
        /// Gets the user's avatar url(40px*40px).
        /// </summary>
        public static string GetAvatar(JObject user) => user.Value<string>("figureurl_qq_1");

        /// <summary>
        /// Gets the user's qq space avatar url(100px*100px).
        /// </summary>
        public static string GetAvatarFull(JObject user) => user.Value<string>("figureurl_qq_2");

        /// <summary>
        /// Gets the user's gender.
        /// </summary>
        public static string GetGender(JObject user) => user.Value<string>("gender");
    }
}
