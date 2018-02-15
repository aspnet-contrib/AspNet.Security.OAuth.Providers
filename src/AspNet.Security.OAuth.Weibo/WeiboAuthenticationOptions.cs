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

            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(WeiboClaimTypes.ScreenName, "screen_name");
            ClaimActions.MapJsonKey(WeiboClaimTypes.ProfileImageUrl, "profile_image_url");
            ClaimActions.MapJsonKey(WeiboClaimTypes.AvatarLarge, "avatar_large");
            ClaimActions.MapJsonKey(WeiboClaimTypes.AvatarHd, "avatar_hd");
            ClaimActions.MapJsonKey(WeiboClaimTypes.CoverImagePhone, "cover_image_phone");
            ClaimActions.MapJsonKey(WeiboClaimTypes.Location, "location");
        }
    }
}
