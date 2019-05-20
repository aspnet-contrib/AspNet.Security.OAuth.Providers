﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;

namespace AspNet.Security.OAuth.Harvest
{
    /// <summary>
    /// Defines a set of options used by <see cref="HarvestAuthenticationHandler"/>.
    /// </summary>
    public class HarvestAuthenticationOptions : OAuthOptions
    {
        public HarvestAuthenticationOptions()
        {
            ClaimsIssuer = HarvestAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(HarvestAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = HarvestAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = HarvestAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = HarvestAuthenticationDefaults.UserInformationEndpoint;


            ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "user", "id");
            ClaimActions.MapCustomJson(ClaimTypes.Name, payload =>
            {
                var user = payload.Value<JObject>("user");
                var parts = new[] { user.Value<string>("first_name"), user.Value<string>("last_name") };
                return string.Join(" ", parts.Where(x => !string.IsNullOrEmpty(x)));
            });
            ClaimActions.MapJsonSubKey(ClaimTypes.GivenName, "user", "first_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Surname, "user", "last_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "user", "email");
        }
    }
}
