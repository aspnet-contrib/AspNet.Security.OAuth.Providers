/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Calendly;

/// <summary>
/// Defines a set of options used by <see cref="CalendlyAuthenticationHandler"/>.
/// </summary>
public class CalendlyAuthenticationOptions : OAuthOptions
{
    public CalendlyAuthenticationOptions()
    {
        ClaimsIssuer = CalendlyAuthenticationDefaults.Issuer;
        CallbackPath = CalendlyAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = CalendlyAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = CalendlyAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = CalendlyAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetProperty("resource").GetString("email"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetProperty("resource").GetString("name"));
    }
}
