/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Weibo
{
    /// <summary>
    /// Defines a set of options used by <see cref="WeiboAuthenticationHandler"/>.
    /// </summary>
    public class WeiboAuthenticationOptions : OAuthOptions
    {
        public WeiboAuthenticationOptions()
        {
            ClaimsIssuer = WeiboAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeiboAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeiboAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeiboAuthenticationDefaults.TokenEndpoint;           
            UserInformationEndpoint = WeiboAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey("urn:weibo:screen_name", "screen_name");
            ClaimActions.MapJsonKey("urn:weibo:profile_image_url", "profile_image_url");
            ClaimActions.MapJsonKey("urn:weibo:avatar_large", "avatar_large");
            ClaimActions.MapJsonKey("urn:weibo:avatar_hd", "avatar_hd");
            ClaimActions.MapJsonKey("urn:weibo:cover_image_phone", "cover_image_phone");
            ClaimActions.MapJsonKey("urn:weibo:location", "location");

            Scope.Add("email");
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; set; } = WeiboAuthenticationDefaults.UserEmailsEndpoint;
    }
}
