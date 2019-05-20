/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.WordPress.WordPressAuthenticationConstants;

namespace AspNet.Security.OAuth.WordPress
{
    /// <summary>
    /// Defines a set of options used by <see cref="WordPressAuthenticationHandler"/>.
    /// </summary>
    public class WordPressAuthenticationOptions : OAuthOptions
    {
        public WordPressAuthenticationOptions()
        {
            ClaimsIssuer = WordPressAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WordPressAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WordPressAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WordPressAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WordPressAuthenticationDefaults.UserInformationEndpoint;

            // Note: limit by default to 'auth' scope only,
            // otherwise too many permissions are requested.
            Scope.Add("auth");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "ID");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(Claims.Email, "email");
            ClaimActions.MapJsonKey(Claims.DisplayName, "display_name");
            ClaimActions.MapJsonKey(Claims.ProfileUrl, "profile_URL");
            ClaimActions.MapJsonKey(Claims.AvatarUrl, "avatar_URL");
            ClaimActions.MapJsonKey(Claims.PrimaryBlog, "primary_blog");
        }
    }
}
