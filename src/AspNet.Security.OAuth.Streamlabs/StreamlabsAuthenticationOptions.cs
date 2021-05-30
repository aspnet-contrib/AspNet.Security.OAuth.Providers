/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using System.Security.Claims;
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

            ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "streamlabs", "id");
            ClaimActions.MapJsonSubKey(ClaimTypes.Name, "streamlabs", "username");
            ClaimActions.MapJsonSubKey(Claims.DisplayName, "streamlabs", "display_name");
            ClaimActions.MapJsonSubKey(Claims.FacebookId, "facebook", "id");
            ClaimActions.MapJsonSubKey(Claims.FacebookName, "facebook", "name");
            ClaimActions.MapJsonSubKey(Claims.Primary, "streamlabs", "primary");
            ClaimActions.MapJsonSubKey(Claims.Thumbnail, "streamlabs", "thumbnail");
            ClaimActions.MapJsonSubKey(Claims.TwitchDisplayName, "twitch", "display_name");
            ClaimActions.MapJsonSubKey(Claims.TwitchIconUrl, "twitch", "icon_url");
            ClaimActions.MapJsonSubKey(Claims.TwitchId, "twitch", "id");
            ClaimActions.MapJsonSubKey(Claims.TwitchName, "twitch", "name");
            ClaimActions.MapJsonSubKey(Claims.YouTubeId, "youtube", "id");
            ClaimActions.MapJsonSubKey(Claims.YouTubeTitle, "youtube", "title");
        }
    }
}
