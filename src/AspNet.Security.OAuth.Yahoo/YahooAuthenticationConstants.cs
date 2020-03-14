/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;

namespace AspNet.Security.OAuth.Yahoo
{
    /// <summary>
    /// Contains constants specific to the <see cref="YahooAuthenticationHandler"/>.
    /// </summary>
    public static class YahooAuthenticationConstants
    {
        public static class Claims
        {
            public const string FamilyName = "urn:yahoo:familyname";
            public const string GivenName = "urn:yahoo:givenname";
            [Obsolete("This has been deprecated. Please use Picture instead. See https://developer.yahoo.com/oauth/social-directory-eol/ for more information.")]
            public const string ImageUrl = "urn:yahoo:profileimage";
            [Obsolete("This has been deprecated. See https://developer.yahoo.com/oauth/social-directory-eol/ for more information.")]
            public const string ProfileUrl = "urn:yahoo:profile";
            public const string Picture = "urn:yahoo:picture";
        }
    }
}
