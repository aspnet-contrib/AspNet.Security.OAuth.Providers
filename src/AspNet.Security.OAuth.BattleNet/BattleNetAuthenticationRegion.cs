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
    /// <summary>
    /// The region for the Americas.
    /// </summary>
    America = 0,

    /// <summary>
    /// The region for China.
    /// </summary>
    China = 1,

    /// <summary>
    /// The region for Europe.
    /// </summary>
    Europe = 2,

    /// <summary>
    /// The region for Korea.
    /// </summary>
    Korea = 3,

    /// <summary>
    /// The region for Taiwan.
    /// </summary>
    Taiwan = 4,

    /// <summary>
    /// The unified global region.
    /// </summary>
    Unified = 5,

    /// <summary>
    /// A custom region. Use this value to use custom endpoints.
    /// </summary>
    Custom = 6,
}
