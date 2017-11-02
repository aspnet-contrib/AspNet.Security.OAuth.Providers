/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Beam
{
    /// <summary>
    /// Defines a set of options used by <see cref="BeamAuthenticationHandler"/>.
    /// </summary>
    public class BeamAuthenticationOptions : OAuthOptions
    {
        public BeamAuthenticationOptions()
        {
            ClaimsIssuer = BeamAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(BeamAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BeamAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BeamAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BeamAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        }
    }
}
