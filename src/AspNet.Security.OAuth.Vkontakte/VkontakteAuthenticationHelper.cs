using System;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Vkontakte
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Facebook after a successful authentication process.
    /// </summary>
    internal static class VkontakteAuthenticationHelper
    {
        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public static string GetId(JObject response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return GetStringValue(response, "uid");
        }
        
        /// <summary>
        /// Gets the first name.
        /// </summary>
        public static string GetFirstName(JObject response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return GetStringValue(response, "first_name");
        }
        
        /// <summary>
        /// Gets the last name.
        /// </summary>
        public static string GetLastName(JObject response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return GetStringValue(response, "last_name");
        }

        /// <summary>
        /// Gets the screen name.
        /// </summary>
        public static string GetScreenName(JObject response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return GetStringValue(response, "screen_name");
        }
        
        /// <summary>
        /// Get photo
        /// </summary>
        public static string GetPhoto(JObject response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return GetStringValue(response, "photo_50");
        }

        private static string GetStringValue(JObject jObject, string key)
        {
            return jObject["response"][0].Value<string>(key) ?? string.Empty;
        }
    }
}