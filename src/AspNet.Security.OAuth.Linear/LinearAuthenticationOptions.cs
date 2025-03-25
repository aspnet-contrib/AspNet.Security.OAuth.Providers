/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Linear.LinearAuthenticationConstants;

namespace AspNet.Security.OAuth.Linear;

/// <summary>
/// Defines a set of options used by <see cref="LinearAuthenticationHandler"/>.
/// </summary>
public class LinearAuthenticationOptions : OAuthOptions
{
    public LinearAuthenticationOptions()
    {
        ClaimsIssuer = LinearAuthenticationDefaults.Issuer;
        CallbackPath = LinearAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = LinearAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = LinearAuthenticationDefaults.TokenEndpointFormat;

        Scope.Add("read");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonSubKey(Claims.OrganizationId, "organization", "id");
        ClaimActions.MapJsonSubKey(Claims.OrganizationName, "organization", "name");
        ClaimActions.MapJsonSubKey(Claims.OrganizationUrlKey, "organization", "urlKey");
    }
}
