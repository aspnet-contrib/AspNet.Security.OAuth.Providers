/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Patreon.PatreonAuthenticationConstants;

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// Defines a set of options used by <see cref="PatreonAuthenticationHandler"/>.
    /// </summary>
    public class PatreonAuthenticationOptions : OAuthOptions
    {
        public PatreonAuthenticationOptions()
        {
            ClaimsIssuer = PatreonAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(PatreonAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = PatreonAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = PatreonAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = PatreonAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("users");
            Scope.Add("pledges-to-me");
            Scope.Add("my-campaign");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");

            ClaimActions.MapCustomJson(
                ClaimTypes.Name,
                user =>
                {
                    if (user.TryGetProperty("attributes", out var attributes))
                    {
                        return attributes.GetString("full_name");
                    }

                    return null;
                });

            ClaimActions.MapCustomJson(
                ClaimTypes.Webpage,
                user =>
                {
                    if (user.TryGetProperty("attributes", out var attributes))
                    {
                        return attributes.GetString("url");
                    }

                    return null;
                });

            ClaimActions.MapCustomJson(
                Claims.Avatar,
                user =>
                {
                    if (user.TryGetProperty("attributes", out var attributes))
                    {
                        return attributes.GetString("thumb_url");
                    }

                    return null;
                });
        }
    }
}
