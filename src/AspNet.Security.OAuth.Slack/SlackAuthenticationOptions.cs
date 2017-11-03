/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Slack
{
    /// <summary>
    /// Defines a set of options used by <see cref="SlackAuthenticationHandler"/>.
    /// </summary>
    public class SlackAuthenticationOptions : OAuthOptions
    {
        public SlackAuthenticationOptions()
        {
            ClaimsIssuer = SlackAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SlackAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SlackAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SlackAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = SlackAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "user");
            ClaimActions.MapJsonKey("urn:slack:team_id", "team_id");
            ClaimActions.MapJsonKey("urn:slack:team_name", "team");
            ClaimActions.MapJsonKey("urn:slack:team_url", "url");
            ClaimActions.MapCustomJson("urn:slack:bot:user_id", user => user["bot"]?.Value<string>("bot_user_id"));
            ClaimActions.MapCustomJson("urn:slack:bot:access_token", user => user["bot"]?.Value<string>("bot_access_token"));
            ClaimActions.MapCustomJson("urn:slack:webhook:channel", user => user["incoming_webhook"]?.Value<string>("channel"));
            ClaimActions.MapCustomJson("urn:slack:webhook:url", user => user["incoming_webhook"]?.Value<string>("url"));
            ClaimActions.MapCustomJson("urn:slack:webhook:configuration_url", user => user["incoming_webhook"]?.Value<string>("configuration_url"));
        }
    }
}
