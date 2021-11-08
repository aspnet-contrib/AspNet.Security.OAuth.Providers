/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.BattleNet;

/// <summary>
/// Defines a list of regions used to determine the appropriate
/// API endpoints when communicating with BattleNet.
/// </summary>
public enum BattleNetAuthenticationRegion
{
    America = 0,
    China = 1,
    Europe = 2,
    Korea = 3,
    Taiwan = 4
}
