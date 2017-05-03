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
    /// instance retrieved from Facebook after a successful authentication process.
    /// </summary>
    public static class WeixinAuthenticationHelper
    {
        /// <summary>
        /// Gets the user's openid.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetOpenId(JObject user) => user.Value<string>("openid");

        /// <summary>
        /// Gets the user's nickname.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetNickname(JObject user) => user.Value<string>("nickname");

        /// <summary>
        /// Gets the user's gender.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetSex(JObject user) => user.Value<string>("sex");

        /// <summary>
        /// Gets the user's province.
        /// </summary>
        public static string GetProvince(JObject user) => user.Value<string>("province");

        /// <summary>
        /// Gets the user's city.
        /// </summary>
        public static string GetCity(JObject user) => user.Value<string>("city");

        /// <summary>
        /// Gets the user's country.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetCountry(JObject user) => user.Value<string>("country");

        /// <summary>
        /// Gets the user's avatar image url.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetHeadimgUrl(JObject user) => user.Value<string>("headimgurl");

        /// <summary>
        /// Gets the user's union id for app.
        /// </summary>
        public static string GetUnionid(JObject user) => user.Value<string>("unionid");

        /// <summary>
        /// Gets the user's privilege information.
        /// </summary>        
        /// <returns></returns>
        public static string GetPrivilege(JObject user)
        {
            var value = user.Value<JArray>("privilege");
            if (value == null)
            {
                return string.Empty;
            }
            return string.Join(",", value.ToObject<string[]>());
        }
    }
}
