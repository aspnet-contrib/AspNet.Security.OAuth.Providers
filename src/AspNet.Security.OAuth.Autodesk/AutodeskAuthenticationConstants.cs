/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Contains constants specific to the <see cref="AutodeskAuthenticationHandler"/>.
    /// </summary>
    public static class AutodeskAuthenticationConstants
    {
        public static class Claims
        {
            public const string EmailVerified = "urn:autodesk:emailverified";
            public const string TwoFactorEnabled = "urn:autodesk:twofactorenabled";
        }
    }
}
