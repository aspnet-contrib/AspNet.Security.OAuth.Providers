/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.GitLab.GitLabAuthenticationConstants;

namespace AspNet.Security.OAuth.GitLab
{
    public class GitLabAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="GitLabAuthenticationOptions"/>.
        /// </summary>
        public GitLabAuthenticationOptions()
        {
            CallbackPath = GitLabAuthenticationDefaults.CallbackPath;
            AuthorizationEndpoint = GitLabAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitLabAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitLabAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");
            Scope.Add("read_user");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.Name, "name");
            ClaimActions.MapJsonKey(Claims.Avatar, "avatar_url");
            ClaimActions.MapJsonKey(Claims.Url, "web_url");
        }
    }
}
