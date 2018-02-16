/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Autodesk.AutodeskAuthenticationConstants;

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Defines a set of options used by <see cref="AutodeskAuthenticationHandler"/>.
    /// </summary>
    public class AutodeskAuthenticationOptions : OAuthOptions
    {
        public AutodeskAuthenticationOptions()
        {
            ClaimsIssuer = AutodeskAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(AutodeskAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AutodeskAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AutodeskAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AutodeskAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("user-profile:read");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "userName");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "emailId");
            ClaimActions.MapJsonKey(Claims.EmailVerified, "emailVerified");
            ClaimActions.MapJsonKey(Claims.TwoFactorEnabled, "2FaEnabled");
        }
    }
}
