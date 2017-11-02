﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Automatic
{
    /// <summary>
    /// Defines a set of options used by <see cref="AutomaticAuthenticationHandler"/>.
    /// </summary>
    public class AutomaticAuthenticationOptions : OAuthOptions
    {
        public AutomaticAuthenticationOptions()
        {
            ClaimsIssuer = AutomaticAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(AutomaticAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AutomaticAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AutomaticAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AutomaticAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Uri, "url");
        }
    }
}