/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Miro.MiroAuthenticationConstants;

namespace AspNet.Security.OAuth.Miro;

public class MiroAuthenticationOptions : OAuthOptions
{
    public MiroAuthenticationOptions()
    {
        ClaimsIssuer = MiroAuthenticationDefaults.Issuer;
        CallbackPath = MiroAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = MiroAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = MiroAuthenticationDefaults.TokenEndpointFormat;
        UserInformationEndpoint = MiroAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "user", "id");
        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "user", "name");
        ClaimActions.MapJsonSubKey(Claims.OrganizationId, "organization", "id");
        ClaimActions.MapJsonSubKey(Claims.OrganizationName, "organization", "name");
        ClaimActions.MapJsonSubKey(Claims.TeamId, "team", "id");
        ClaimActions.MapJsonSubKey(Claims.TeamName, "team", "name");
    }
}
