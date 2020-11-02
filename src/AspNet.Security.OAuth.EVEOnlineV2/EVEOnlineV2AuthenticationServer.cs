/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth
{
    /// <summary>
    /// Defines a list of servers used to determine the appropriate
    /// API endpoints when communicating with EVEOnlineV2.
    /// </summary>
    public enum EVEOnlineV2AuthenticationServer
    {
        /// <summary>
        /// Live server.
        /// </summary>
        Tranquility = 0,

        /// <summary>
        /// Test server.
        /// </summary>
        Singularity = 1
    }
}
