/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Constants used with authentication in Autodesk provider
    /// </summary>
    public static class AutodeskAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// Key for claim containing a boolean value indicating whether the user's email has been verified
            /// </summary>
            public const string EmailVerified = "urn:autodesk:emailverified";

            /// <summary>
            /// Key for claim containing a boolean value indicating whether the user has enabled two factor authentication
            /// </summary>
            public const string TwoFactorEnabled = "urn:autodesk:twofactorenabled";
        }

        public static class Scopes
        {
            /// <summary>
            /// OAuth scope for reading Autodesk user profile
            /// </summary>
            public const string UserProfileRead = "user-profile:read";
        }
    }
}
