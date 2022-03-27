/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Naver;

/// <summary>
/// Contains constants specific to the <see cref="NaverAuthenticationHandler"/>.
/// </summary>
public static class NaverAuthenticationConstants
{
    public static class Claims
    {
        /// <summary>
        /// The claim for the user's nickname.
        /// </summary>
        public const string Nickname = "urn:naver:nickname";

        /// <summary>
        /// The claim for the user's age range.
        /// </summary>
        public const string Age = "urn:naver:age";

        /// <summary>
        /// The claim for the user's year of birth
        /// </summary>
        public const string YearOfBirth = "urn:naver:birthyear";

        /// <summary>
        /// The claim for the user's profile image url
        /// </summary>
        public const string ProfileImage = "urn:naver:profile_image";
    }
}
