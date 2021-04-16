﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.WorkWeixin.WorkWeixinAuthenticationConstants;

namespace AspNet.Security.OAuth.WorkWeixin
{
    /// <summary>
    /// Defines a set of options used by <see cref="WorkWeixinAuthenticationHandler"/>.
    /// </summary>
    public class WorkWeixinAuthenticationOptions : OAuthOptions
    {
        public WorkWeixinAuthenticationOptions()
        {
            ClaimsIssuer = WorkWeixinAuthenticationDefaults.Issuer;
            CallbackPath = WorkWeixinAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = WorkWeixinAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WorkWeixinAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WorkWeixinAuthenticationDefaults.UserInformationEndpoint;
            UserIdentificationEndpoint = WorkWeixinAuthenticationDefaults.UserIdentificationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.Avator, "avator");
            ClaimActions.MapJsonKey(Claims.Mobile, "mobile");
        }

        /// <summary>
        /// <see cref="AgentId"/>
        /// </summary>
        public string AgentId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL of the user identification endpoint(a.k.a the "OpenID endpoint")
        /// </summary>
        public string UserIdentificationEndpoint { get; set; }
    }
}
