/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Fitbit
{
    /// <summary>
    /// Contains constants specific to the <see cref="FitbitAuthenticationHandler"/>.
    /// </summary>
    public static class FitbitAuthenticationConstants
    {
        public static class Claims
        {
            public const string Avatar = "urn:fitbit:avatar";
            public const string Avatar150 = "urn:fitbit:avatar150";
        }
    }
}
