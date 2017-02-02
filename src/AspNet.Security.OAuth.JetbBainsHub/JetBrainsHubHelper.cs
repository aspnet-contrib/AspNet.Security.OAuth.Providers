using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.JetbBainsHub {
    public static class JetBrainsHubHelper {
        public static string GetId(JObject user) {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("id");
        }

        public static string GetName(JObject user) {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return TryGetValue(user, "user", "name");
        }

        public static string GetEmail(JObject user) {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return TryGetValue(user, "email", "email");
        }

        // Get the given subProperty from a property.
        private static string TryGetValue(JObject user, string propertyName, string subProperty)
        {
            JToken value;
            if (user.TryGetValue(propertyName, out value))
            {
                var subObject = JObject.Parse(value.ToString());
                if (subObject != null && subObject.TryGetValue(subProperty, out value))
                {
                    return value.ToString();
                }
            }
            return null;
        }

        // Get the given subProperty from a list property.
        private static string TryGetFirstValue(JObject user, string propertyName, string subProperty)
        {
            JToken value;
            if (user.TryGetValue(propertyName, out value))
            {
                var array = JArray.Parse(value.ToString());
                if (array != null && array.Count > 0)
                {
                    var subObject = JObject.Parse(array.First.ToString());
                    if (subObject != null)
                    {
                        if (subObject.TryGetValue(subProperty, out value))
                        {
                            return value.ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}