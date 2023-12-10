/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WorldID;

/// <summary>
/// Contains constants specific to the <see cref="WorldIDAuthenticationHandler"/>.
/// </summary>
public static class WorldIDAuthenticationConstants
{
    public static class Claims
    {
        public const string CredentialType = "urn:worldid:credential_type";
        public const string LikelyHuman = "urn:worldid:likely_human";
    }
}
