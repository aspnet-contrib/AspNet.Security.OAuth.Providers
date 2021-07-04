/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.AdobeIO
{
    /// <summary>
    /// Contains constants specific to the <see cref="AdobeIOAuthenticationHandler"/>.
    /// </summary>
    public static class AdobeIOAuthenticationConstants
    {
        public static class Claims
        {
            public const string AccountType = "urn:adobeio:account_type";
            public const string EmailVerified = "urn:adobeio:email_verified";
        }
    }
}
