/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.CiscoSpark
{
    /// <summary>
    /// Defines a set of options used by <see cref="CiscoSparkAuthenticationHandler"/>.
    /// </summary>
    public class CiscoSparkAuthenticationOptions : OAuthOptions
    {
        public CiscoSparkAuthenticationOptions()
        {
            ClaimsIssuer = CiscoSparkAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(CiscoSparkAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = CiscoSparkAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = CiscoSparkAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = CiscoSparkAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.Value<JArray>("emails")?.First?.Value<string>());
        }
    }
}
