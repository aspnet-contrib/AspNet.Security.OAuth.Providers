/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Slack.SlackAuthenticationConstants;

namespace AspNet.Security.OAuth.Slack;

/// <summary>
/// Defines a set of options used by <see cref="SlackAuthenticationHandler"/>.
/// </summary>
public class SlackAuthenticationOptions : OAuthOptions
{
    public SlackAuthenticationOptions()
    {
        ClaimsIssuer = SlackAuthenticationDefaults.Issuer;

        CallbackPath = SlackAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = SlackAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = SlackAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = SlackAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "user", "name");
        ClaimActions.MapJsonSubKey(ClaimTypes.Email, "user", "email");
        ClaimActions.MapJsonSubKey(Claims.UserId, "user", "id");
        ClaimActions.MapJsonSubKey(Claims.TeamId, "team", "id");
        ClaimActions.MapJsonSubKey(Claims.TeamName, "team", "name");
        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, (element) =>
        {
            string? teamId = null;
            string? userId = null;

            if (element.TryGetProperty("team", out var team))
            {
                teamId = team.GetString("id");
            }

            if (element.TryGetProperty("user", out var user))
            {
                userId = user.GetString("id");
            }

            return $"{teamId}|{userId}";
        });

        Scope.Add("identity.basic");
    }
}
