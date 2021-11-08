/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Kloudless.KloudlessAuthenticationConstants;

namespace AspNet.Security.OAuth.Kloudless;

/// <summary>
/// Defines a set of options used by <see cref="KloudlessAuthenticationHandler"/>.
/// </summary>
public class KloudlessAuthenticationOptions : OAuthOptions
{
    public KloudlessAuthenticationOptions()
    {
        ClaimsIssuer = KloudlessAuthenticationDefaults.Issuer;
        CallbackPath = KloudlessAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = KloudlessAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = KloudlessAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = KloudlessAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "account");
        ClaimActions.MapJsonKey(Claims.Account, "account");
        ClaimActions.MapJsonKey(Claims.Service, "service");
        ClaimActions.MapJsonKey(Claims.InternalUse, "internal_use");
        ClaimActions.MapJsonKey(Claims.Created, "created");
        ClaimActions.MapJsonKey(Claims.Modified, "modified");
        ClaimActions.MapJsonKey(Claims.ServiceName, "service_name");
        ClaimActions.MapJsonKey(Claims.Admin, "admin");
        ClaimActions.MapJsonKey(Claims.Apis, "apis");
        ClaimActions.MapJsonKey(Claims.EffectiveScope, "effective_scope");
        ClaimActions.MapJsonKey(Claims.Api, "api");
        ClaimActions.MapJsonKey(Claims.Type, "type");
        ClaimActions.MapJsonKey(Claims.Enabled, "enabled");
        ClaimActions.MapJsonKey(Claims.ObjectDefinitions, "object_definitions");
        ClaimActions.MapJsonKey(Claims.CustomProperties, "custom_properties");
        ClaimActions.MapJsonKey(Claims.ProxyConnection, "proxy_connection");
        ClaimActions.MapJsonKey(Claims.Active, "active");

        Scope.Add(Scopes.Any);
    }
}
