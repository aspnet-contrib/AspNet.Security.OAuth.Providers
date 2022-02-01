/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.BungieNet;

/// <summary>
/// Contains constants specific to the <see cref="BungieNetAuthenticationHandler"/>.
/// </summary>
public static class BungieNetAuthenticationConstants
{
    public static class Claims
    {
        public const string ProfilePicturePath = "profilePicturePath";
        public const string UniqueName = "uniqueName";
    }
}
