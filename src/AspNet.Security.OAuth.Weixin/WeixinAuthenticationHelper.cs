/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Weixin
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Weixin after a successful authentication process.
    /// </summary>
    public static class WeixinAuthenticationHelper
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public static string GetOpenId(JObject user) => user.Value<string>("openid");

        /// <summary>
        /// Gets the nickname associated with the user profile.
        /// </summary>
        public static string GetNickname(JObject user) => user.Value<string>("nickname");

        /// <summary>
        /// Gets the gender associated with the user profile.
        /// </summary>
        public static string GetSex(JObject user) => user.Value<string>("sex");

        /// <summary>
        /// Gets the province associated with the user profile.
        /// </summary>
        public static string GetProvince(JObject user) => user.Value<string>("province");

        /// <summary>
        /// Gets the city associated with the user profile.
        /// </summary>
        public static string GetCity(JObject user) => user.Value<string>("city");

        /// <summary>
        /// Gets the country associated with the user profile.
        /// </summary>
        public static string GetCountry(JObject user) => user.Value<string>("country");

        /// <summary>
        /// Gets the avatar image url associated with the user profile.
        /// </summary>
        public static string GetHeadimgUrl(JObject user) => user.Value<string>("headimgurl");

        /// <summary>
        /// Gets the union id associated with the application.
        /// </summary>
        public static string GetUnionid(JObject user) => user.Value<string>("unionid");

        /// <summary>
        /// Gets the privilege associated with the user profile.
        /// </summary>
        public static string GetPrivilege(JObject user)
        {
            var value = user.Value<JArray>("privilege");
            if (value == null)
            {
                return null;
            }

            return string.Join(",", value.ToObject<string[]>());
        }
    }
}
