/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Streamlabs.StreamlabsAuthenticationConstants;

namespace AspNet.Security.OAuth.Streamlabs
{
    /// <summary>
    /// Defines a set of options used by <see cref="StreamlabsAuthenticationHandler"/>.
    /// </summary>
    public class StreamlabsAuthenticationOptions : OAuthOptions
    {
        public StreamlabsAuthenticationOptions()
        {
            ClaimsIssuer = StreamlabsAuthenticationDefaults.Issuer;
            CallbackPath = StreamlabsAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = StreamlabsAuthenticationDefaults.AuthorizationEndPoint;
            TokenEndpoint = StreamlabsAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = StreamlabsAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => GetData(user, "streamlabs.id"));
            ClaimActions.MapCustomJson(ClaimTypes.Name, user => GetData(user, "streamlabs.username"));
            ClaimActions.MapCustomJson(Claims.DisplayName, user => GetData(user, "streamlabs.display_name"));
            ClaimActions.MapCustomJson(Claims.FacebookId, user => GetData(user, "facebook.id"));
            ClaimActions.MapCustomJson(Claims.FacebookName, user => GetData(user, "facebook.name"));
            ClaimActions.MapCustomJson(Claims.Primary, user => GetData(user, "streamlabs.primary"));
            ClaimActions.MapCustomJson(Claims.Thumbnail, user => GetData(user, "streamlabs.thumbnail"));
            ClaimActions.MapCustomJson(Claims.TwitchDisplayName, user => GetData(user, "twitch.display_name"));
            ClaimActions.MapCustomJson(Claims.TwitchIconUrl, user => GetData(user, "twitch.icon_url"));
            ClaimActions.MapCustomJson(Claims.TwitchId, user => GetData(user, "twitch.id"));
            ClaimActions.MapCustomJson(Claims.TwitchName, user => GetData(user, "twitch.name"));
            ClaimActions.MapCustomJson(Claims.YoutubeId, user => GetData(user, "youtube.id"));
            ClaimActions.MapCustomJson(Claims.YoutubeTitle, user => GetData(user, "youtube.title"));
        }

        private static string? GetData(JsonElement user, string key)
        {
            user = GetJsonElement(user, key);

            return user.ValueKind != JsonValueKind.Null && user.ValueKind != JsonValueKind.Undefined ?
              user.ToString() :
              default;
        }

        private static JsonElement GetJsonElement(JsonElement jsonElement, string path)
        {
            if (jsonElement.ValueKind == JsonValueKind.Null ||
                jsonElement.ValueKind == JsonValueKind.Undefined)
            {
                return default;
            }

            string[] segments = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int n = 0; n < segments.Length; n++)
            {
                jsonElement = jsonElement.TryGetProperty(segments[n], out JsonElement value) ? value : default;

                if (jsonElement.ValueKind == JsonValueKind.Null ||
                    jsonElement.ValueKind == JsonValueKind.Undefined)
                {
                    return default;
                }
            }

            return jsonElement;
        }
    }
}
