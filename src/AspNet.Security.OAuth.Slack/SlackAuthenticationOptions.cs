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

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user =>
                string.Concat(user["team"]["id"], "|", user["user"]["id"]));
            ClaimActions.MapJsonSubKey(ClaimTypes.Name, "user", "name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "user", "email");
            ClaimActions.MapJsonSubKey("urn:slack:user_id", "user", "id");
            ClaimActions.MapJsonSubKey("urn:slack:team_id", "team", "id");
            ClaimActions.MapJsonSubKey("urn:slack:team_name", "team", "name");

            Scope.Add("identity.basic");
        }
    }
}
