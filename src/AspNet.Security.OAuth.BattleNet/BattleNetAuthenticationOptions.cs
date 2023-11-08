/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.BattleNet;

/// <summary>
/// Defines a set of options used by <see cref="BattleNetAuthenticationHandler"/>.
/// </summary>
public class BattleNetAuthenticationOptions : OAuthOptions
{
    public BattleNetAuthenticationOptions()
    {
        ClaimsIssuer = BattleNetAuthenticationDefaults.Issuer;
        CallbackPath = BattleNetAuthenticationDefaults.CallbackPath;

        Region = BattleNetAuthenticationRegion.Unified;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "battletag");
    }

    /// <summary>
    /// Sets the region used to determine the appropriate API endpoints when communicating
    /// with BattleNet. The default value is <see cref="BattleNetAuthenticationRegion.Unified"/>.
    /// </summary>
    public BattleNetAuthenticationRegion Region { get; set; }
}
