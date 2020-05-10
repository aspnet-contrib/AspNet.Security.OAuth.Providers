/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Used to map usage to specific online environment, i.e. development, stage or production.
    /// </summary>
    public enum SuperOfficeAuthenticationEnvironment
    {
        Development,
        Stage,
        Production
    }
}
