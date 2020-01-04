/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.DingTalk.DingTalkAuthenticationConstants;

namespace AspNet.Security.OAuth.DingTalk
{
    /// <summary>
    /// Defines a set of options used by <see cref="DingTalkAuthenticationHandler"/>.
    /// </summary>
    public class DingTalkAuthenticationOptions : OAuthOptions
    {
        public DingTalkAuthenticationOptions()
        {
            ClaimsIssuer = DingTalkAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(DingTalkAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DingTalkAuthenticationDefaults.AuthorizationEndpoint;
            UserInformationEndpoint = DingTalkAuthenticationDefaults.UserInformationEndpoint;
            TokenEndpoint = DingTalkAuthenticationDefaults.TokenEndpoint;

            Scope.Add("snsapi_login");

            ClaimActions.MapJsonKey(ClaimTypes.Name, "nick");
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(Claims.UnionId, "unionid");
            ClaimActions.MapJsonKey(Claims.OpenId, "openid");
            ClaimActions.MapJsonKey(Claims.DingId, "dingId");
            ClaimActions.MapJsonKey(Claims.MainOrgAuthHighLevel, "main_org_auth_high_level");
        }

        public bool IsUseUserPasswordLogin
        {
            get { return AuthorizationEndpoint == DingTalkAuthenticationDefaults.AuthorizationUserPassEndpoint; }
            set { AuthorizationEndpoint = value ? DingTalkAuthenticationDefaults.AuthorizationUserPassEndpoint : DingTalkAuthenticationDefaults.AuthorizationEndpoint; }
        }

        public string AppId
        {
            get => ClientId;
            set => ClientId = value;
        }

        public string AppSecret
        {
            get => ClientSecret;
            set => ClientSecret = value;
        }
    }
}

