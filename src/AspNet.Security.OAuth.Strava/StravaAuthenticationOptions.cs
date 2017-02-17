/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Strava
{
    /// <summary>
    /// Defines a set of options used by <see cref="StravaAuthenticationHandler"/>.
    /// </summary>
    public class StravaAuthenticationOptions : OAuthOptions
    {
        public StravaAuthenticationOptions()
        {
            AuthenticationScheme = StravaAuthenticationDefaults.AuthenticationScheme;
            DisplayName = StravaAuthenticationDefaults.DisplayName;
            ClaimsIssuer = StravaAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(StravaAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = StravaAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = StravaAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = StravaAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("public");
        }
    }
}
