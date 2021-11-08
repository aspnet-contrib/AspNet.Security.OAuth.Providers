/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Moodle;

/// <summary>
/// Contains constants specific to the <see cref="MoodleAuthenticationHandler"/>.
/// </summary>
public static class MoodleAuthenticationConstants
{
    public static class Claims
    {
        public const string IdNumber = "urn:moodle:idnumber";
        public const string MoodleId = "urn:moodle:id";
        public const string Language = "urn:moodle:lang";
        public const string Description = "urn:moodle:desc";
    }
}
