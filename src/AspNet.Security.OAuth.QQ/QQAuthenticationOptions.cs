/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.QQ
{
    /// <summary>
    /// Defines a set of options used by <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public class QQAuthenticationOptions : OAuthOptions
    {
        public QQAuthenticationOptions()
        {
            ClaimsIssuer = QQAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(QQAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = QQAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQAuthenticationDefaults.TokenEndpoint;
            UserIdentificationEndpoint = QQAuthenticationDefaults.UserIdentificationEndpoint;
            UserInformationEndpoint = QQAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("get_user_info");

            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(QQClaimTypes.PictureUrl, "figureurl");
            ClaimActions.MapJsonKey(QQClaimTypes.PictureMediumUrl, "figureurl_1");
            ClaimActions.MapJsonKey(QQClaimTypes.PictureFullUrl, "figureurl_2");
            ClaimActions.MapJsonKey(QQClaimTypes.AvatarUrl, "figureurl_qq_1");
            ClaimActions.MapJsonKey(QQClaimTypes.AvatarFullUrl, "figureurl_qq_2");
        }

        /// <summary>
        /// Gets or sets the URL of the user identification endpoint (aka "OpenID endpoint").
        /// </summary>
        public string UserIdentificationEndpoint { get; set; }
    }
}
