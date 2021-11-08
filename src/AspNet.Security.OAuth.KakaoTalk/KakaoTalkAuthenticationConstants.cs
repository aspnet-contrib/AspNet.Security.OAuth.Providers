/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.KakaoTalk;

/// <summary>
/// Contains constants specific to the <see cref="KakaoTalkAuthenticationHandler"/>.
/// </summary>
public static class KakaoTalkAuthenticationConstants
{
    public static class Claims
    {
        /// <summary>
        /// The claim for the user's age range.
        /// </summary>
        public const string AgeRange = "urn:kakaotalk:age_range";

        /// <summary>
        /// The claim for the user's year of birth
        /// </summary>
        public const string YearOfBirth = "urn:kakaotalk:year_of_birth";
    }
}
