/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Fitbit.FitbitAuthenticationConstants;

namespace AspNet.Security.OAuth.Fitbit
{
    /// <summary>
    /// Defines a set of options used by <see cref="FitbitAuthenticationHandler"/>.
    /// </summary>
    public class FitbitAuthenticationOptions : OAuthOptions
    {
        public FitbitAuthenticationOptions()
        {
            ClaimsIssuer = FitbitAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(FitbitAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = FitbitAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = FitbitAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = FitbitAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("profile");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "encodedId");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
            ClaimActions.MapJsonKey(Claims.Avatar150, "avatar150");
        }
    }
}
