/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.GitCode.GitCodeAuthenticationConstants;

namespace AspNet.Security.OAuth.GitCode;

/// <summary>
/// Defines a set of options used by <see cref="GitCodeAuthenticationHandler"/>.
/// </summary>
public class GitCodeAuthenticationOptions : OAuthOptions
{
    public GitCodeAuthenticationOptions()
    {
        ClaimsIssuer = GitCodeAuthenticationDefaults.Issuer;
        CallbackPath = GitCodeAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = GitCodeAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = GitCodeAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = GitCodeAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(Claims.AvatarUrl, "avatar_url");
        ClaimActions.MapJsonKey(Claims.Bio, "bio");
        ClaimActions.MapJsonKey(Claims.Blog, "blog");
        ClaimActions.MapJsonKey(Claims.Company, "company");
        ClaimActions.MapJsonKey(Claims.HtmlUrl, "html_url");
        ClaimActions.MapJsonKey(Claims.Name, "name");
    }
}
