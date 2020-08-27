/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.ExactOnline
{
    /// <summary>
    /// Contains constants specific to the <see cref="ExactOnlineAuthenticationHandler"/>.
    /// </summary>
    public static class ExactOnlineAuthenticationConstants
    {
        public static class Claims
        {
            public const string Division = "urn:exactonline:division";
            public const string Company = "urn:exactonline:company";
        }
    }
}
