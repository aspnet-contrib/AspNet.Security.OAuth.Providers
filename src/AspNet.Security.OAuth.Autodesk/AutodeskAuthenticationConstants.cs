/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Autodesk
{
    /// <summary>
    /// Constants used by the Autodesk authentication middleware.
    /// </summary>
    public static class AutodeskAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// Name of the claim containing a boolean value indicating
            /// whether the user's email has been verified.
            /// </summary>
            public const string EmailVerified = "urn:autodesk:emailverified";

            /// <summary>
            /// Name of the claim containing a boolean value indicating
            /// whether the user has enabled two factor authentication.
            /// </summary>
            public const string TwoFactorEnabled = "urn:autodesk:twofactorenabled";
        }

        public static class Scopes
        {
            /// <summary>
            /// Name of the OAuth2 scope used for reading Autodesk user profile.
            /// </summary>
            public const string UserProfileRead = "user-profile:read";
        }
    }
}
